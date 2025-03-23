import pandas as pd
import plotly.express as px
from flask import Flask, render_template_string
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

# Map old domain names to new domain names
domain_name_mapping = {
    "JobVacancy": "JobVacancy",
    "Medical": "Medical",
    "Sports": "Sports"
}
df['Domain'] = df['domain'].map(domain_name_mapping)

# Define different thresholds for each domain
domain_thresholds = {
    "JobVacancy": 0.6,
    "Medical": 0.3,
    "Sports": 0.7
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
        labels={'FileIndex': 'FileName1 (Source)', 'SimilarityScore': 'Similarity Score'},
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

# Create a Flask app
app = Flask(__name__)

@app.route('/')
def index():
    return render_template_string('''
        <html>
            <head>
                <title>Phrase Similarity by Domain</title>
            </head>
            <body>
                <h1>Semantic Similarity Classification by Domain</h1>
                <h2>Semantic Similarity For JobRequirement Relevance</h2>
                {{ jobvacancy_plot|safe }}
                <h2>Semantic Similarity For Medical - Best Possible Medications for Patients Based on Diagnosis History</h2>
                {{ medical_plot|safe }}
                <h2>Semantic Similarity For Sports- Comparing Famous Legends with News Articles</h2>
                {{ sports_plot|safe }}
            </body>
        </html>
    ''', jobvacancy_plot=domain_plots.get('JobVacancy', ''),
                                  medical_plot=domain_plots.get('Medical', ''),
                                  sports_plot=domain_plots.get('Sports', ''))

if __name__ == '__main__':
    app.run(debug=True)
