import plotly.graph_objects as go
import pandas as pd
import numpy as np

# Read embedding values from two input CSV files
collection1 = pd.read_csv('preprocessed_JobProfileCDeveloper.txtembedding_values.csv', header=None).values.flatten()
collection2 = pd.read_csv('preprocessed_JobRequirement.txtembedding_values1.csv', header=None).values.flatten()

# Similarity score (Replace with actual score if needed)
similarity_score = 0.6865949

# Generate scalar values for x-axis
scalar_values = np.arange(len(collection1))

# Create a DataFrame
df = pd.DataFrame({
    'ScalarValues': scalar_values,
    'Collection1': collection1,
    'Collection2': collection2
})

# Plot using Plotly
fig = go.Figure()

# Scatter plot for Collection 1
fig.add_trace(go.Scatter(
    x=df['ScalarValues'], y=df['Collection1'],
    mode='markers+lines', name='preprocessed_JobProfileCDeveloper.txt (Collection 1)',
    marker=dict(color='blue', size=5),
    line=dict(color='blue', width=2)
))

# Scatter plot for Collection 2
fig.add_trace(go.Scatter(
    x=df['ScalarValues'], y=df['Collection2'],
    mode='markers+lines', name='preprocessed_JobRequirement.txt (Collection 2)',
    marker=dict(color='green', size=5),
    line=dict(color='green', width=2)
))

# Similarity score as horizontal line
fig.add_hline(y=similarity_score, line_dash="dash", line_color="red",
              annotation_text=f"Similarity Score: {similarity_score}",
              annotation_position="top right")

# Customize layout
fig.update_layout(
    title='Comparison of Two Word Document Embeddings with Similarity Score',
    xaxis_title='Values (0 to 3072)',
    yaxis_title='Cosine Similarity Score',
    template='plotly_white',
    legend_title='Collections',
    height=600,
    width=1000
)

# Show the plot
fig.show()
