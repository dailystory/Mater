{
	layout: '_ArticleLayout',
	title: 'Welcome to Mater',
	description: 'Mater is a super-simple GitHub-based doc system'
}
# Welcome to Mater
Mater is a super-simple, open source, GitHub-based doc system.

Instead of requiring a complex content management system, which typically requires a database, Mater uses [Markdown](https://en.wikipedia.org/wiki/Markdown) files stored and versioned in GitHub and auto-published in an Azure Web App.

Mater is open source and was originally developed by for use as [DailyStory's](https://dailystory.com) documentation system.

## Live Demo
[See Mater powering DailyStory's documentation](https://docs.dailystory.com) from [DailyStory's Docs Repository](https://github.com/dailystory/docs).

## Auto-Publishing Documentation
Mater makes it insanely easy to publish documentation. 

Mater is designed to work with Microsoft Azure's GitHub integration. When new documentation files are checked in they are automatically published to your documentation website running Mater.

Documentation is versioned and maintained in GitHub. Plus, documentation can be edited using virtually any document editing application.

[Instructions for setting up Azure auto-publishing](/auto-publish/azure)

## Getting Started
Getting started with Mater is simple. 

The only requirement is that Mater expects a physcial or virtual directory **\articles** and to find both an **\articles\index.md** file and a **\articles\TOC.md** file.

Within the \articles directory you add markdown files (any file name eding with .md), images, and other resources.

You can create sub-folders with their own index.md. For example, \articles\auto-publish\azure.md is accessible as [/auto-publish/azure](/auto-publish/azure).

Markdown files can be referenced directly by removing the .md extension. For example, \articles\demos\hello.md is accessible as [/demos/hello](/demos/hello).

Images added to your directories and referenced in your markdown files.

## Setting Page Title, Description and More
You can add a Json string to the start of your markdown file to tell Mater the page title, description, and more.

For example, you can set the &lt;title&gt; of you page by including {title:"My Title"}.

## Customizing Mater's UI
Mater is built using the [Bootstrap framework](http://getbootstrap.com/), but you can easily add your own CSS, favicon, Google tags and more to Mater.

Simply create a head.html file - you'll find an example provided in the \articles folder. Content in head.html will be automatically included in the &lt;head&gt; section of your pages.

## Why Mater?
git-r-done.
