This project performs semantic similarity analysis on textual documents using .NET C#. It has been extended to run as a cloud-native Azure Function in a Docker container. The system is triggered by messages in Azure Queue Storage, processes ZIP files stored in Azure Blob Storage, and outputs results to Azure Table Storage.

Architecture Overview

 Flow:

Code is developed as an Azure Function and published in a Docker container.

User uploads a ZIP file containing training data to Azure Blob Storage.

User sends a message to Azure Queue to trigger training.

Function reads ZIP from Blob, extracts data, and processes it.

Function writes output to Azure Table Storage.

Azure Setup

Create Storage Account

Blob Container: training-data

Queue: training-queue

Table: results

Upload ZIP File

Upload ZIP with documents to training-data blob container.

Send Queue Message

Message contains metadata or file reference (e.g., blob name).

