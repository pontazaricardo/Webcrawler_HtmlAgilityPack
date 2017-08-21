# Webcrawler_HtmlAgilityPack

This is an example of how to crawl a website using the (NuGet) HtmlAgilityPack and saving the results to a text file.

## Installation

In order to install the **HtmlAgilityPack** module to your project, in Visual studio do the following:
1. Go to *Tools* -> *NuGet Package manager* -> *Manage Nuget Packages for Solution...*
2. Search for *HtmlAgilityPack* in the search bar.  
3. When found, click your project and then click on *Install*.
4. In the *Review Changes* window, click on OK.

The above steps will install the **HtmlAgilityPack** in your project. A confirmation *readme.txt* will be displayed. In order to be completely sure that your project now has the **HtmlAgilityPack**, check your project's *References*.

![installation](/images/installation.gif?raw=true)

## How to crawl a website

For this project, the website [http://www.espn.com/nba/statistics](http://www.espn.com/nba/statistics) was crawled as an example. At the moment of the development of this project, this website had the following tables:
1. Offensive Leaders,
2. Defensive Leaders,
3. Assists,
4. Blocks, 
5. Field Goal, and
6. Steals

with the ranking, name, hyperlink and points of each player. After analyzing the HTML, some defined structures are needed to be identified. For this project, the structures:
```html
<div class="mod-container mod-table mod-no-footer">
```
and 
```html
<div class="mod-container mod-table mod-no-footer mod-no-header">
```
were spotted. These two structures are the ones that contain the players' data.

![data](/images/htmlSections.gif?raw=true)