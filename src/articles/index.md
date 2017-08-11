---
layout: _ArticleLayout
title: Welcome to Mater
description: Mater is a super-simple GitHub-based doc system
---
# Welcome to Mater
Mater is a super-simple, open source, GitHub-based doc system.

Instead of requiring a complex content management system, which typically requires a database, Mater uses [Markdown](https://en.wikipedia.org/wiki/Markdown) files stored and versioned in GitHub.

Mater is open source and was originally developed by [Rob Howard](https://twitter.com/robhoward) for use as [DailyStory's](https://dailystory.com) documentation system.

## Auto-Publishing Documentation
Mater makes it insanely easy to publish documentation. It was originally designed to work with Microsoft Azure's GitHub integration - when new documentation files are checked in they are automatically deployed to your documentation site running Mater.

[Instructions for setting up Azure auto-publishing](/publish/azure)

## Getting Started
Getting started with Mater is easy. 

> Important - Mater requires a physcial or virtual directory **\articles** and expects to find both an **index.md** file and a **TOC.md** file.

* Add Markdown files, images and other resources to your \articles directory.
* You can create sub-folders with their own index.md. For example, \articles\publish\azure.md is accessible as [/publish/azure](/publish/azure).
* Other Markdown files can be referenced directly by removing the .md extension. For example, \articles\demos\hello.md is accessible as [/demos/hello](/demos/hello)