# StockForThePeople

![image](https://github.com/user-attachments/assets/c2b4f25f-5593-4fde-aac9-e69a975484a0)

Bespreken met Rick:
- mijn 'service' doet de vertaalslag tussen DTO en domein modellen. zowel van buiten naar binnen als van binnen naar buiten. Doordat de service een referentie heeft naar die modellen, hebben de webapi en het blazorproject dat allebei ook. Is dat okay? dat je frontend toegang heeft tot je domein modellen?
- hoe zou jij omgaan met een job die 1 x per dag moet draaien (het ophalen van de koersen)
- wat zou een goeie unit test zijn om te maken
- 

The goal of this application is to allow casual investors to analyze the progress of their portfolio.
Ever notice how you can't really track how well one asset is doing compared to another? That's on purpose.
Analytical tools for small investors are usually hidden behind subscriptions.

This application attempts to give some of those insights without requiring a subscription fee.
Of course this comes with a downside. There are no minute to minute updates.
But if you are like me, you don't need that. What you need is a daily insight or even a weekly insight.

Questions like: 
- "Hey, the financial sector is up in the last month, but energy transition is down. Is that a common pattern?"
- "Hmm, last year food prices went down in september, maybe I should sell a bit of that in july?"
- "Which stock did better over the last 3 months compared to the others in my portfolio?"
- If there is a lot of trading on a certain stock, does that necessarily say something about its value?


Done:
- Set up several projects to separate concerns from the get go.
- Set up serilog
- First version of domain models and external DTOs
- ExternalData service receives and processes data
- InternalData service feeds the WebApi
- Set up first database from EF core
- Use both postman and insomnia for testing
- Use chart.js
- setup a simple outputcache in the WebApi

- Store data:
- Monthly (manual process)
- Historically - last 1000 days (manual process)

- First graph page done
- 
Next:
Technically:
- build first component library for more separation of concerns and reusability
- think about making separate chart.js components
- think about making a generic linechart that can handle multiple or single incoming data arrays
- work on making the api calls (internal and external) a bit more fault tolerant. (cancellation tokens, alert actions)
- think about a good way to have the external update service just trigger on a (?) timer
- Make 2 services (one for when blazor is running serverside and the other for when blazor is running webassembly) to make the blazor pages & components cleaner.
- see if I can do a simple unit test that gets data from an external mock api, puts it in memory database, and retrieves it from the api.

Next:
Functionally:
- work on another graph page: does the difference between high/low say something really about volatility of value
- work on another graph page: multiselect assets and show if their volume and or value correlate
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
- 
