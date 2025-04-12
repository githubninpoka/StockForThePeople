# StockForThePeople

![image](https://github.com/user-attachments/assets/c8fd008c-c813-4f7e-aa77-1330fa85458e)


**Disclaimer: work in progress. the code is still messy. redundant DTO's and endpoints because I'm very much building and refactoring.**

The goal of this application is to allow casual investors to analyze the progress of their portfolio.
Ever notice how you can't really track how well one asset is doing compared to another? That's on purpose.
Analytical tools for small investors are usually hidden behind subscriptions.

This application attempts to give some of those insights without requiring a subscription fee.
Of course this comes with a downside. There are no minute to minute updates.
But if you are like me, you don't need that. What you need is a daily insight or even a weekly insight.

Questions like: 
- Hey, the financial sector is up in the last month, but energy transition is down. Is that a common pattern?
- Hmm, last year food prices went down in september, maybe I should sell a bit of that in july?
- Which stock did better over the last 3 months compared to the others in my portfolio?
- If there is a lot of trading on a certain stock, does that necessarily say something about its value?
- If there is a lot of trading on a certain stock, does that say something about the daily difference between low and high?
- Can I find assets that regularly fluctuate in a negatively correlated way? (if one asset drops, does the other normally rise?)


Done:
- Set up several projects to separate concerns from the get go.
- Set up serilog
- First version of domain models and external DTOs
- ExternalData service receives, processes and stores data
- InternalData service feeds the WebApi (the WebApi is used by the Blazor frontend)
- Set up first database from EF core
- Use both postman and insomnia for testing
- Use chart.js
- setup a simple outputcache in the WebApi
- Decided to NOT implement a separate Component Library for now
- in the multi asset functionality, implement Task.Whenall for calling multiple endpoints

- Store data:
- Monthly (manual process)
- Historically - last 1000 days (manual process)

- Visually:
- First graph page done
- Make a generic linechart that can handle multiple or single incoming data arrays
- rework the layout away from standard MainLayout
- 4 working Graph pages
- - Volume compared to value for a single asset
- - Volume compared to high low fluctuation for a single asset
- - Relative volume compared to relative value for a single asset
- - Relative value of multiple assets over time

Next:
Technically:

- Won't do now: build first component library for more separation of concerns and reusability
- think about making separate chart.js components (more than just a single Linechart component)
- work on making the api calls (internal and external) a bit more fault tolerant. (cancellation tokens, alert actions) -- not necessary at this stage though
- think about a good way to have the external update service just trigger on a (?) timer
- Make 2 services (one for when blazor is running serverside and the other for when blazor is running webassembly) to make the blazor pages & components cleaner.
- see if I can do a simple unit test that gets data from an external mock api, puts it in memory database, and retrieves it from the api.
- implement nicer expiring caching with etag etc. (see rick morty project for reference)
- fix having to press the update button twice on relative multival page before graph updates (probably a lifecycle issue i'm overlooking for now)
- fix the way the StockForThePeopleSettings are now loaded in the server side project, because I have to adjust now in 2 places instead of 1 (program.cs and usersecrets)
- do thorough testruns in both webassembly and servermode (especially with autorendermode) as right now everything is only running servermode, but forcing webassembly looks good too!

Next:
Functionally:
- ~~work on page: Does volume say something about volatility of value~~
- ~~work on another graph page: does the difference between high/low say something really about volatility of value~~
- ~~work on another graph page: multiselect assets and show if their~~ volume ~~and or value~~ correlate
- see if there are any other more precise external data sources available

Patterns & design:
- options pattern
- service pattern
- inversion of control

Learnt:
- be very aware of the lifecycle of razor pages
- you can't call Java Interop in dispose
- javascript is very much case sensitive (hard lesson)
- webassembly does not use appsettings in the same way as other asp .net core applications (hard lesson)
- when creating an anonymous object, it is possible to vary the type of a property (because javascript is not strongly typed and c# is)
- bootstrap pulldowns, popovers and tooltips *require* the bootstrap javascript to be also loaded.
- there is no pretty multi select component available out of the box in either Blazor or Bootstrap.

Good books:
- Web Development with Blazor, Third Edition, Copyright © 2024 Packt Publishing
- - Reiterating over all the concepts that I already knew
	- Explained how to setup a component library in blazor.

 
Onderzoeken:
- !! How to design the endpoints of my API. I have multiple sets of information that need calculation (business rules) and I want to avoid repeated calls to the database while performing a single calculation each time. So do I do single endpoint with query parameters or multiple endpoints, which will mean an explosion of endpoints. -> after consulting with some seniors common sense prevailed: I split the endpoints where it was logical and do not fuss anymore about some query parameters.
- mijn 'service' doet de vertaalslag tussen DTO en domein modellen. zowel van buiten naar binnen als van binnen naar buiten. Doordat de service een referentie heeft naar die modellen, hebben de webapi en het blazorproject dat allebei ook. Is dat okay? dat je frontend toegang heeft tot je domein modellen? -> after consulting, this is an okay solution.
- hoe zou jij omgaan met een job die 1 x per dag moet draaien (het ophalen van de koersen)
- wat zou een goeie unit test zijn om te maken
- nog iets over DTO's. In mijn solution heb ik in het blazor project een DTO folder met DTOs. Als ik wil werken met een Component Library, dan moet die Component Library kennis hebben van de DTOs. Maar ik kan geen circulaire project referentie opnemen natuurlijk. Is het normaal om, naast je domeinmodellen, ook je DTOs in een apart project te plaatsen? Ook als die 'alleen' bedoeld zijn voor, in mijn geval, de externe data naar binnen halen (WebApi extern), het response model te maken (WebApi intern) en data te tonen in Blazor (Blazor Componenten). -> after discussion: it is okay to have DTOs in both the blazor project and the component library. I'm still wondering about duplication of code though.
- 
