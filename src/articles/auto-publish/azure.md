{
title: 'Auto-Publish with Microsoft Azure',
description: 'Setting up auto-publishing with Microsoft Azure'
}
# Auto-Publish with Microsoft Azure
Auto-publishing your documentation with Azure is what Mater was built to do.

![Microsoft Azure](/articles/auto-publish/azure.png "Microsoft Azure")

## Setup Mater with Microsoft Azure
Configuring Mater on Microsoft Azure is simple:

### 1. Create a new Azure Web App.
First, create a new Azure Web App

![New Azure Web App](/articles/auto-publish/azure-01.png "New Azure Web App")

### 2. Publish/FTP Mater into your new Web App.
Next, publish Mater into your new Azure Web App into the default root directory: site\wwwroot

### 3. Create a GitHub repository
The GitHub repository can be public or private. You can copy the index.md and TOC.md files from Mater to use for your initial check-in.

You can also use [DailyStory's GitHub docs](https://github.com/dailystory/docs).

### 4. Configure your Web App to use a GitHub deployment option.
Next, configure your Azure Web App to use a GitHub deployment:

![Configure GitHub](/articles/auto-publish/azure-02.png "Configure GitHub")

And select GitHub:

![Configure GitHub](/articles/auto-publish/azure-03.png "Configure GitHub")

You will need to select a repository that contains your documentation files, such as the one created in step 3.

![Configure GitHub](/articles/auto-publish/azure-04.png "Configure GitHub")

### 5. Add an articles virtual directory
Create a virtual directory in your Azure web app that points to the proper path of your GitHub repository.

When you completed step 4 Azure will update site\repository on your Azure Web App whenever you update your documentation repository.

For example, [DailyStory's docs are published here](https://github.com/dailystory/docs). When this GitHub repository is deployed to Azure the files are added and updated in the site\repository directory. 

> Since DailyStory's repository contains an 'articles' folder we need to map our virtual directory as folows:

![Virtual directory](/articles/auto-publish/azure-05.png "Virtual directory")

### 5. Start writing docs
Now, you can start writing your docs in Markdown managing them all through GitHub. When you commit a doc file in GitHub it will automatically update your Mater documentation site.