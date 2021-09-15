# Todo Web

A not-so-trivial note-taking app to show how [Microsoft Tye](https://github.com/dotnet/tye) helps develop, test, and deploy microservices today (September 2021).

I developed this app for my talk "_Improving productivity of microservices development with Project Tye_" at [Programmers' Week 2021](https://cognizantsoftvisionevents.com/programmersweek/techtalks).

It consists of:
- A frontend (.NET 5 Razor Pages).
- A RESTful API (.NET 5 Web Api), than handles CRUD operations and communicates with a gRPC service to manage the keywords in the notes you take.
- A gRPC service (.NET 5 gRPC service), that stores and retrieves keywords from a cache.

Additionally, ``tye.yaml`` defines a Redis cache as a dependency.

In the different commits, you can see how the app evolved from isolated services that use in-memory data to something close to a final product that incorporates Tye features, such as service discovery and automatic management of dependencies (a Redis cache in this case).