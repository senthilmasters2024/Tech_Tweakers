import pandas as pd
import plotly.express as px

# Load your dataset (ensure the CSV is loaded if needed)
df = pd.read_csv("phrase_similarity.csv")

# Ensure 'Relevance' column exists
df["Relevance"] = df["SimilarityScore"].apply(lambda x: "Relevant" if x >= 0.5 else "Irrelevant")

# Create scatter plot using Plotly
fig = px.scatter(
    df,
    x="Domain",
    y="SimilarityScore",
    color="Relevance",
    hover_data=["Phrase1", "Phrase2", "SimilarityScore", "Domain"],
    title="Phrase Similarity Classification by Domain"
)

# Add a threshold line at 0.5
fig.add_hline(y=0.5, line_dash="dash", line_color="blue")

# Update layout for better visualization
fig.update_layout(
    xaxis_title="Domain",
    yaxis_title="Similarity Score",
    legend_title="Relevance",
    template="plotly_white"
)

# Show the plot
fig.show()
