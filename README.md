# Mater
Mater is a super-simple, open source, GitHub-based doc system.

Instead of requiring a complex content management system, which typically requires a database, Mater uses [Markdown](https://en.wikipedia.org/wiki/Markdown) files stored and versioned in GitHub.

Mater is open source and was originally developed by [Rob Howard](https://twitter.com/robhoward) for use as [DailyStory's](https://dailystory.com) documentation system.

## Auto-Publishing Documentation
Mater makes it insanely easy to publish documentation. It was originally designed to work with Microsoft Azure's GitHub integration - when new documentation files are checked in they are automatically deployed to your documentation site running Mater.

[Instructions for setting up Azure auto-publishing](https://github.com/dailystory/Mater/blob/master/src/articles/auto-publish/azure.md)

## Getting Started
Getting started with Mater is easy. 

> Important - Mater requires a physcial or virtual directory **\articles** and expects to find both an **index.md** file and a **TOC.md** file.

* Add Markdown files, images and other resources to your \articles directory.
* You can create sub-folders with their own index.md. For example, \articles\auto-publish\azure.md is accessible as /auto-publish/azure.
* Other Markdown files can be referenced directly by removing the .md extension. For example, \articles\demos\hello.md is accessible as /demos/hello.

## Setting title, description and more 
If your markdown file starts with a Json string, Mater will read that Json string for settings.

For example, you can set the &lt;title&gt; of you page by including {title:"My Title"}.

## Customizing Mater's UI
Mater is built using the [Bootstrap framework](http://getbootstrap.com/). 

You can easily add your own CSS, favicon, Google tags and more to Mater.

Create a head.html file, such as the one in the \articles folder. Content in head.html will be automatically included in the &lt;head&gt; section of your content.

## Why Mater?
git-r-done.
