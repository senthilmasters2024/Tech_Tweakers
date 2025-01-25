import pandas as pd
import plotly.express as px

# Data from your example
phrases = [
    ("Angela Merkel", "Government", "Politics", 0.219175514),
    ("Cristiano Ronaldo", "Government", "Sports", 0.103471597),
    ("Dog", "Cat", "Animals", 0.627560688),
    ("Medicine", "Paracetamol", "Medical", 0.351968035),
    ("Cricket", "Sachin Tendulkar", "Sports", 0.413716178),
    ("Cricket", "Sports", "Sports", 0.508625698),
    ("Cricket", "Yorker", "Sports", 0.377595062),
    ("Tesla", "Electric Vehicle", "Energy", 0.457574398),
    ("AI Surveillance", "Privacy", "Surveillance", 0.283669489),
    ("Heart Surgery", "Cardiologist", "Medical", 0.450601618),
    ("Soccer", "FIFA World Cup", "Sports", 0.475371313),
    ("Lion", "Predator", "Animals", 0.397817926),
    ("Renewable Energy", "Solar Panel", "Energy", 0.466463275),
    ("Basketball", "Michael Jordan", "Sports", 0.447001607),
    ("Data Encryption", "Cybersecurity", "Technology", 0.445288531),
    ("Water Quality", "pH Levels", "Water Management", 0.415180898),
    ("COVID-19", "Vaccine", "Medical", 0.364522045),
    ("Eiffel Tower", "Tourism", "Culture", 0.267087231),
    ("Football", "Goalkeeper", "Sports", 0.416252891),
    ("Polar Bear", "Arctic", "Animals", 0.518373095),
    ("Machine Learning", "Neural Networks", "Technology", 0.574667639),
    ("Hydropower", "Dam", "Energy", 0.260465791),
    ("Marathon", "Runner", "Sports", 0.427633885),
    ("Smart Home", "IoT Devices", "Technology", 0.503448914),
    ("Asthma", "Inhaler", "Medical", 0.48573991),
    ("Wildfire", "Deforestation", "Environment", 0.409494224),
    ("Chess", "Magnus Carlsen", "Sports", 0.421859028),
    ("Elephant", "Ivory", "Animals", 0.485246874),
    ("Climate Change", "Carbon Emissions", "Environment", 0.529501603),
    ("Olympics", "Gold Medal", "Sports", 0.555795579),
    ("Artificial Intelligence", "Deep Learning", "Technology", 0.502116796),
    ("Rainwater Harvesting", "Sustainability", "Water Management", 0.310762707),
    ("Diabetes", "Insulin", "Medical", 0.575739162),
    ("Satellite", "Space Exploration", "Technology", 0.397992175),
    ("Python", "Programming", "Technology", 0.530121793),
    ("Yoga", "Meditation", "Wellness", 0.635605993),
    ("Shark", "Ocean", "Animals", 0.34157135),
    ("Renewable Energy", "Wind Turbine", "Energy", 0.470634443),
    ("Gymnastics", "Balance Beam", "Sports", 0.484068887),
    ("Vaccination", "Immunity", "Medical", 0.449350377),
    ("Blockchain", "Cryptocurrency", "Technology", 0.479787455),
    ("Amazon Rainforest", "Biodiversity", "Environment", 0.332482011),
    ("Olympic Games", "Tokyo 2020", "Sports", 0.550825764),
    ("Tiger", "Endangered", "Animals", 0.329025647),
    ("Electric Vehicle", "Charging Station", "Energy", 0.478433041),
    ("Renewable Energy", "Geothermal", "Energy", 0.467655001),
    ("Plastic Pollution", "Marine Life", "Environment", 0.334877754),
    ("Marathon", "Boston Marathon", "Sports", 0.647929126),
    ("Cardiology", "Stent", "Medical", 0.256186021),
    ("Ecosystem", "Biodiversity", "Environment", 0.497503003),
    ("E-sports", "DOTA 2", "Technology", 0.456167233)
]


# Convert to DataFrame
df = pd.DataFrame(phrases, columns=['Phrase1', 'Phrase2', 'Domain', 'SimilarityScore'])

# Set threshold for relevance
threshold = 0.5
df['Relevance'] = df['SimilarityScore'].apply(lambda x: 'Relevant' if x >= threshold else 'Irrelevant')

# Create a scatter plot with Plotly
fig = px.scatter(
    df,
    x='Domain',
    y='SimilarityScore',
    color='Relevance',
    hover_data=['Phrase1', 'Phrase2'],  # Adds Phrase1 and Phrase2 to hover info
    title='Phrase Similarity Classification by Domain',
    labels={'Domain': 'Domain', 'SimilarityScore': 'Similarity Score'}
)

# Add threshold line
fig.add_shape(
    type="line",
    x0=-0.5, x1=len(df['Domain'].unique()) - 0.5,  # Extends across all x-axis categories
    y0=threshold, y1=threshold,
    line=dict(color="Blue", dash="dash"),
    xref="x", yref="y"
)

# Show the plot
fig.show()
