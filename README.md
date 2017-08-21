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

After identifying the needed sections, the website crawl can be done in three easy steps.

## Crawling code

### Loading the main site

The first step is to crawl the main site. In order to do this, the code
```c#
string mainSite = "http://www.espn.com/nba/statistics";
HtmlWeb site = new HtmlWeb();
HtmlDocument htmlDocument = site.Load(@mainSite);
```
is needed. Please note that for these commands to work, you need to do the import:
```c#
using HtmlAgilityPack;
```

### Parsing individual sections

Now that the main site has been crawled, we need to access the individual sections containing the data we need. For this project, we use the code
```c#
HtmlNodeCollection leaderBoards_01 = htmlDocument.DocumentNode.SelectNodes("//div[@class='mod-container mod-table mod-no-footer']"); //We need only the first two.
HtmlNodeCollection leaderBoards_02 = htmlDocument.DocumentNode.SelectNodes("//div[@class='mod-container mod-table mod-no-footer mod-no-header']"); //We will use all of them
```
This code will create a *HtmlNodeCollection* object that contains all the *<div>* sections of the form
```html
<div class="mod-container mod-table mod-no-footer">
<div class="mod-container mod-table mod-no-footer mod-no-header">
```

#### Notes

About the *HtmlNodeCollection* objects:
1. If you use the double slash notation *//* before any HTML tag, it will look for these tags from the beginning of the site.
2. If you don't use the *//* tags, it will look from the parent object.

So for example
```c#
HtmlNodeCollection collection = Parent.SelectNodes("//div[@class='foo bar']");
```
will look starting from the main site (and probably will find *<div>* that are not inside *Parent*), and
```c#
HtmlNodeCollection collection = Parent.SelectNodes("div[@class='foo bar']");
```
will look for the *<div>* inside *Parent* only (in one level).

### Construction of objects

After reaching the HTML level that contains the needed data, it can be obtained as normal values by using the **Attributes** or **InnerText** methods as
```c#
HtmlNode cell_name_aTag = cell_name.SelectNodes("a").FirstOrDefault();

string link = cell_name_aTag.Attributes["href"].Value.ToString();
string name = cell_name_aTag.InnerText;
```