---
layout: _ArticleLayout
title: Auto-Publish with Microsoft Azure
description: Setting up auto-publishing with Microsoft Azure
---
# Auto-Publish with Microsoft Azure
Auto-publishing your documentation with Azure is what Mater was built to do.

![Microsoft Azure](/articles/auto-publish/azure.png "Microsoft Azure")

## Setup Mater with Microsoft Azure
Configuring Mater on Microsoft Azure is simple:

### 1. Create a new Azure Web App.

### 2. Publish/FTP Mater into your new Web App.

### 3. Configure your Web App to use a GitHub deployment option.

### 4. Add an articles virtual directory
Create a virtual directory in your Azure web app that points to the proper path of your GitHub repository.

For example, [DailyStory's docs are published here](https://github.com/dailystory/docs). When this GitHub repository is deployed to Azure the files are added and updated in the site\repository directory. 

Since the repository contains an 'articles' folder we need to map our virtual directory as folows:

![Virtual directory](/setup/auto-publish/azure-05.png "Virtual directory")

### 5. Start writing docs
Now, you can start writing your docs in Markdown managing them all through GitHub. When you commit a doc file in GitHub it will automatically update your Mater documentation site.