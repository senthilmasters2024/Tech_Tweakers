import pandas as pd
import plotly.express as px
from flask import Flask, render_template_string, request
import os

# Get the path of the current script
current_directory = os.path.dirname(os.path.abspath(__file__))

# Define the relative path to your file
file_path = os.path.join(current_directory, 'data', 'output_dataset.csv')

# Load the dataset
df = pd.read_csv(file_path)

# Ensure required columns exist
required_columns = {'FileName1', 'FileName2', 'domain', 'SimilarityScore'}
if not required_columns.issubset(df.columns):
    raise ValueError(f"Dataset must contain columns: {required_columns}. Found columns: {df.columns.tolist()}")

# Convert SimilarityScore to float if it's not already
df['SimilarityScore'] = pd.to_numeric(df['SimilarityScore'], errors='coerce')

# Ensure values are within -1 to 1
df['SimilarityScore'] = df['SimilarityScore'].clip(-1, 1)


# Correct domain mapping - Handling both types of filenames
def map_domain(domain):
        if 'JobProfile' in domain or 'JobRequirement' in domain:
            return "JobVacancy"
        elif 'Medical-MedicationSuggestion' in domain:
            return "Medical"
        else:
            return "Unknown"


df['Domain'] = df['FileName1'].apply(map_domain)

# Create a Flask app
app = Flask(__name__)


@app.route('/', methods=['GET', 'POST'])
def index():
    # Default thresholds
    default_thresholds = {
        "JobVacancy": 0.6,
        "Medical": 0.3,
        "Unknown": 0.7
    }

    # If POST request, update thresholds from user input
    if request.method == 'POST':
        job_vacancy_threshold = float(request.form.get('job_vacancy_threshold', default_thresholds['JobVacancy']))
        medical_threshold = float(request.form.get('medical_threshold', default_thresholds['Medical']))
        sports_threshold = float(request.form.get('sports_threshold', default_thresholds['Unknown']))
    else:
        job_vacancy_threshold = default_thresholds['JobVacancy']
        medical_threshold = default_thresholds['Medical']
        sports_threshold = default_thresholds['Unknown']

    # Apply user-defined thresholds
    domain_thresholds = {
        "JobVacancy": job_vacancy_threshold,
        "Medical": medical_threshold,
        "Sports": sports_threshold
    }

    # Apply domain-specific thresholds to classify relevance
    df['Relevance'] = df.apply(
        lambda row: 'Relevant' if row['SimilarityScore'] >= domain_thresholds.get(row['Domain'], 0.6) else 'Irrelevant',
        axis=1)

    # Create individual scatter plots for each domain
    domain_plots = {}
    for domain in df['Domain'].unique():
        domain_df = df[df['Domain'] == domain].copy()
        domain_df['FileIndex'] = range(len(domain_df))

        fig = px.scatter(
            domain_df,
            x='FileIndex',
            y='SimilarityScore',
            color='Relevance',
            hover_data={'FileName1': True, 'FileName2': True, 'Domain': True, 'SimilarityScore': True},
            title=f'Semantic Similarity Classification for {domain} (Threshold = {domain_thresholds[domain]})',
            labels={'FileIndex': 'Sources - Number of Comparisons', 'SimilarityScore': 'Similarity Score'},
            size_max=10
        )

        # Add threshold line
        fig.add_shape(
            type="line",
            x0=0,
            x1=len(domain_df) - 1,
            y0=domain_thresholds[domain], y1=domain_thresholds[domain],
            line=dict(color="black", dash="dash"),
            xref="x", yref="y",
        )

        # Ensure y-axis displays range from -1 to 1
        fig.update_layout(
            xaxis=dict(title='Sources - Number of Comparisons', tickangle=45),
            yaxis=dict(title='Similarity Score', range=[-1, 1]),
            legend_title_text='Relevance',
            showlegend=True
        )

        domain_plots[domain] = fig.to_html(full_html=False)

    return render_template_string('''
        <html>
            <head>
                <title>Phrase Similarity by Domain</title>
            </head>
            <body>
                <h1>Semantic Similarity Classification by Domain</h1>

                <form method="POST">
                    <h3>Set Thresholds for Each Domain:</h3>
                    <label>Job Vacancy Threshold:</label>
                    <input type="number" name="job_vacancy_threshold" step="0.01" value="{{ job_vacancy_threshold }}"><br>

                    <label>Medical Threshold:</label>
                    <input type="number" name="medical_threshold" step="0.01" value="{{ medical_threshold }}"><br>

                    <label>Sports Threshold:</label>
                    <input type="number" name="sports_threshold" step="0.01" value="{{ sports_threshold }}"><br><br>

                    <input type="submit" value="Update Thresholds">
                </form>

                {% if domain_plots.get('JobVacancy') %}
                    <h2>Semantic Similarity For Job Requirement Relevance</h2>
                    {{ domain_plots['JobVacancy']|safe }}
                {% endif %}

                {% if domain_plots.get('Medical') %}
                    <h2>Semantic Similarity For Medical - Best Possible Medications</h2>
                    {{ domain_plots['Medical']|safe }}
                {% endif %}

                {% if domain_plots.get('Sports') %}
                    <h2>Semantic Similarity For Sports - Comparing News Articles</h2>
                    {{ domain_plots['Sports']|safe }}
                {% endif %}
            </body>
        </html>
    ''',
                                  job_vacancy_threshold=job_vacancy_threshold,
                                  medical_threshold=medical_threshold,
                                  sports_threshold=sports_threshold,
                                  domain_plots=domain_plots)


if __name__ == '__main__':
    app.run(debug=True)
