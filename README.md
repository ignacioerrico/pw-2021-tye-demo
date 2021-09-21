Hi, and thanks for dropping by!

This is a summary of the talk I gave on September 15, 2021, at [Programmers' Week 2021](https://cognizantsoftvisionevents.com/programmersweek/techtalks), "_Improving productivity of microservices development with Project Tye_".

Here's a [simplified version of the slide deck I used](https://github.com/ignacioerrico/pw-2021-tye-demo/blob/main/doc/PW2021-Tye.pdf).

# Prelude

Modern software solutions rarely consist of a single application. In the urge to increase the speed and efficiency of developing and managing software solutions at scale, _microservices_ and _cloud-native_ have become buzzwords. They introduced new challenges too, and working with multiple applications introduced some chores.
- Was it hard to get up to speed when you joined your latest project?
- Do you regularly do things that you feel could be automated?
- Is there a more efficient way to work with microservices?

# What is Tye?

- Tye is a **developer tool** that makes developing, testing, and deploying microservices and distributed applications easier.
- Tye is a **.NET tool**. A .NET tool is simply a special NuGet package that contains a console application. In other words, it's a command line tool. However, as we'll see below, there is a frontend. There is also an [official plugin for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-tye), as well as an [unofficial extension for Visual Studio](https://marketplace.visualstudio.com/items?itemName=ikkentim.TimsTyeExplorer).
- Tye is an **open source experiment at Microsoft**. Being an experiment means that the future of what happens to Tye is not clear. The development team is trying out radical ideas for now in order to improve microservices developer productivity.
  - Keep in mind that many of Microsoft's major products started out as a project. (During my talk I mentioned _Project Avalon_. Does it ring a bell? I wouldn't be surprised if it doesn't! But what if I saw WPF? Well, WPF started out as Project Avalon.)
  - So Tye is an experiment, but it's paving the way for a new way to work.
- Tye is [hosted on Github](https://github.com/dotnet/tye). At the time of this writing, prerelease version `0.9.0-alpha.21380` is available, and that's the version I used during my presentation.

# Getting Tye

Tye is a **.NET tool**, so it's essentially a NuGet package, as I mentioned before. Head over to [nuget.org](https://www.nuget.org/packages/Microsoft.Tye/). Copy and paste the .NET CLI Global command you see there into a command prompt, hit Enter, and you'll have Tye on your machine in a jiffy!

# Case study

A great way to get started with Tye is the [tutorial in the official documentation](https://github.com/dotnet/tye/tree/main/docs#tutorials). The goal of my talk was to take that a step further, and show what it's like to develop a microservices application with Tye and to deploy it to a local Kubernetes cluster—or to the could, for that matter.

In the interest of focusing on Tye, I developed a somewhat simple application, but complex enough to give you an idea of some of the problems that Tye is trying to solve. It's a note-taking app I decided to call _Todo Web_.

## Todo Web

![A simple diagram of the architecture of the application](https://github.com/ignacioerrico/pw-2021-tye-demo/blob/main/doc/a-not-so-trivial-app.jpg "A simple diagram of the architecture of the application")

Components at a glance:
- A frontend (.NET 5 Razor Pages).
- A RESTful API (.NET 5 Web Api). Handles CRUD operations and communicates with a gRPC service to manage the keywords in the notes you take.
- A gRPC service (.NET 5 gRPC service). Stores and retrieves keywords from a cache.

In the different commits, you can see how the app evolved from isolated services that use in-memory data to something close to a final product that incorporates Tye features, such as service discovery and automatic management of dependencies (a Redis cache in this case).

> This might as well be an e-commerce application or ticket-booking system for an airline. It doesn't matter. I want to show you what it's like to develop this application with Tye, so don't worry if you don't know some of the technologies involved, like gRPC.

**Note**: the idea is that gRPC is used for performance, but I admit this is a bit overkill for this application. It was an excuse to have at least two services, but that's really beside the point. It's all about Tye, not about the application per se.

# This is what you want to read!

Enough talking, let's get hands-on!

Install [Tim's Tye Explorer extension for Visual Studio](https://marketplace.visualstudio.com/items?itemName=ikkentim.TimsTyeExplorer).

Clone this repo and run:

`git checkout f5c70f727c0794d802ab05e7bf8e7d4afe17116a`

What you just did is open up Visual Studio, add new project, choose the .NET 5 Razor Pages Web App template, and write some code that uses in-memory data. Well done!

You would normally run the app to see what it looks like (with `F5` in Visual Studio or with `dotnet run` on the command line), debug the application if something doesn't work as expected, make changes, and run again. We all do that.

## Running the web app with Tye

Now let's do the same, but with Tye.

In the root folder of the cloned repo, issue these commands:

`cd src\01-Frontend`

`tye run Todo.Web --watch`

> The `--watch` modifier will watch for any code changes and automatically rebuild those changes.

Read the output from Tye. You'll see it mentions a dashboard.

Open [localhost:8000](http://localhost:8000). That's the **Tye Dashboard**, where you have an overview of all the services in your application. For now we just have a single web app, but as we add microservices, the dashboard will become more populated.

- Click on `todo-web` (the name of the service). You'll see a bunch of **metrics** that are being updated in real time (this is related to that last line in the console output, "_Listening for event pipes_.")
  > `EventPipe` is a new component of the .NET Core Runtime, that is, it's a CoreCLI component, that allows you to consume events emitted by the runtime (so for example by the Just-In-Time compiler or the garbage collector). It allows you to collect tracing data without having to rely on platform-specific OS-native components. In other words, EventPipes give us consistency across platforms.
- Take a look at the **logs**. This is the exact same output you get when we run the app with `dotnet run`, but this time the server is listening on ports randomly assigned by Tye.
- Click on one of the **bindings**. There's your app!

Make changes to the code and save them. You'll see immediately in the console that the changes are detected and the app is being rebuilt. Refresh the web app to see your changes.

In Visual Studio go to _View_ → _Other Windows_ → _Tye Explorer_. Click on `todo-web` and then on `Attach to selected`. You are now debugging the app. (Normally you would need to go to _Debug_ &rarr; _Attach to Process..._, and look for your process.)

> Something I noticed is that you have to manually refresh Tye Explorer when the process ID of one of your services changes, which happens when a service is rebuilt. In other words, in order to attach the debugger to a service in Tye Explorer, you need to refresh first, so that it picks up the correct process ID of the service.

Feel free to experiment with the code and with the app. All data is stored in memory, so once you restart the app, everything is back to square one. In order to do that, stop Tye with `CTRL+C` on the terminal.

## Adding more services

Stop Tye (`CTRL+C`) and go to the root folder of the solution (where the `.sln` file is). Let's say you added two microservices to the application (a RESTful API and a gRPC service), as well as a class library and unit tests.

Run:

`tye run --watch`

This time you'll see all services on the dashboard. Notice that neither the class library nor the unit test project are there. The dashboard is an overview of your services (and other dependencies—more on that later).

Clicking on the binding for `todo-api` will open Swagger. However, clicking on the binding for `words-grpc` will give you a 404 error, because the gRPC service doesn't listen for incoming HTTP requests.

> I actually cheated a bit in the Web API solution. By default, the endpoint for Swagger is in the path `/swagger`. [I changed that default to the root path](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/f5c70f727c0794d802ab05e7bf8e7d4afe17116a#diff-e8819a759bf1ed41f7afc8029bf81c2dadffdb7ab2e03ca1f58d772003c19bafR73), so that clicking on the binding wouldn't give a 404. That's a [current shortcoming in Tye: inability to define one or more paths of interest](https://github.com/dotnet/tye/issues/618). Bindings on the dashboard points to the root path, and there is currently no way to change that.

Again, it's all in-memory data, so you can experiment with it.

How do you like having a dashboard for your services?

Do note that you didn't have to configure anything. You just did `tye run`, and all your services were up and running. (Yes, it's possible to do that without Tye, but remember, the goal is to improve your productivity.)

## Make services talk to each other

Write some more code:

`git checkout 7aa8752118f53b6970ecc8b8bda15c4e58aff608`

> If you have local changes, undo them with these two commands before checking out that commit:
>
> `git reset --hard`
>
> `git clean -f`

You just _removed_ [the repository with hard-coded data](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/7aa8752118f53b6970ecc8b8bda15c4e58aff608#diff-5f3ff96586592329116c54944801b431d40962c54f3754f45fb0dd02e158f930) and [the in-memory cache](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/7aa8752118f53b6970ecc8b8bda15c4e58aff608#diff-d40e71f0d8829bc2abf1cd8f4d03ff95a51033a0f02714c5537d516eb39d1af6) in the front-end. You _removed_ [the hard-coded URI in appsettings.json](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/7aa8752118f53b6970ecc8b8bda15c4e58aff608#diff-4f570d2b158038d4cbc08fe7c402c415f0638742cb34feacb2ac6d728be8cfe8L2) too.

The front-end now talks to the API. But how? You're using Tye's **service discovery** capabilities.
- You added the NuGet package `Microsoft.Tye.Extensions.Configuration` to:
  - [the front-end](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/7aa8752118f53b6970ecc8b8bda15c4e58aff608#diff-cdfda5999fd7a6c097c1294097d67cad4a989adc174b28b29b85f9b33ad1b9d6R9),
  - [the API](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/7aa8752118f53b6970ecc8b8bda15c4e58aff608#diff-1a3ee9f94104251e557604ff4105e77ec1574decc1daa8a063f18464626ee314R21), and
  - [the gRPC service](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/7aa8752118f53b6970ecc8b8bda15c4e58aff608#diff-b03318ecc1d91289724c5331409e8f9d77bb1e6bc10b47d67851c959ab3cd295R13).
- You used the method `GetServiceUri()` to get the URI of the service you need:
  - when configuring the front-end services, specifically [when adding an HTTP Client to specify the base address](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/7aa8752118f53b6970ecc8b8bda15c4e58aff608#diff-635227cc4e7b1121ffe20a816df493b21670c7d22f8ae9640459d418fa76a688R31),
  - when configuring the RESTful API services, specifically [when setting the address of the gRPC service for the client](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/7aa8752118f53b6970ecc8b8bda15c4e58aff608#diff-e8819a759bf1ed41f7afc8029bf81c2dadffdb7ab2e03ca1f58d772003c19bafR33).

That's all.

Run `tye run` again. Click on one of the bindings for the front-end. The data now comes from the back-end. It's hard-coded in the back-end, but now the services use service discovery to know how to reach the ones they need to contact.

> It's important to mention that **Tye doesn't force you** to use its own solution for service discovery. You're free to implement service discovery differently.

## Taking control

So far, all we did was `tye run` and things just worked. No configuration needed. Tye chose (random and unused) ports for each service for us. What if we need to use a specific port in a certain environment, which wouldn't be something too farfetched to think of?

Run:

`tye init`

That creates the file `tye.yaml` with the default configuration used implicitly by Tye. You can fine-tune that configuration to suit your needs. It's a single point of configuration and it ultimately describes your application.

Going over each and every configuration setting is well beyond the scope of this document. I highly encourage you to check the [tye.yaml schema](https://github.com/dotnet/tye/blob/main/docs/reference/schema.md) in the official documentation.

But you can try these changes:

`git checkout 49b00e02216ca5db91b4b2434498c2f471d243ef`

You just specified [two replicas for the front-end](https://github.com/ignacioerrico/pw-2021-tye-demo/commit/49b00e02216ca5db91b4b2434498c2f471d243ef#diff-cbc33f9a2d863e8cb06546d9534bf32d37a6eb2d4fedf65852c72160e7d6f4dbR11) in the `tye.yaml` file.

Run `tye run` again, and you'll see you have two replicas for the front-end. Tye adds a sidecar process that performs load balancing. You can see which replica is selected at runtime by checking the metrics. The replica that had work to do will have higher memory values.

### Excercises

1. Define an environment variable with the `env` element and read it in the code.

    `tye.yaml`:
    
    ```yaml
    # ...
    - name: todo-web
      project: src/01-Frontend/Todo.Web/Todo.Web.csproj
      replicas: 2
      env:                       # Add these two lines
      - MaxNumberOfTodoNotes=5   #
    # ...
    ```
    
    Then, display it somewhere in the front-end:
    
    `Index.cshtml`:
    
    ```cs
    @Environment.GetEnvironmentVariable("MaxNumberOfTodoNotes")
    ```

2. Create different configuration files. This should give you an idea of what Tye brings to the table when it comes to deployment.

    ```dos
    copy tye.yaml tye.dev.yaml
    copy tye.yaml tye.prod.yaml
    ```

    Try different configurations in the two new files. Then run:

    ```dos
    tye run tye.dev.yaml
    tye run tye.prod.yaml
    ```

3. Use **tags** to define which services should be put into effect.

    ```yaml
    # ...
    - name: todo-web-dev
      project: src/01-Frontend/Todo.Web/Todo.Web.csproj
      env:
      - MaxNumberOfTodoNotes=5
      tags:
      - DEV
    - name: todo-web
      project: src/01-Frontend/Todo.Web/Todo.Web.csproj
      env:
      - MaxNumberOfTodoNotes=10
      tags:
      - PROD
    # ...

    ```

    Then use the `--tags` modifier with the tag that fits the bill:

    ```dos
    tye run --tags PROD
    ```

    > Using tags can yield a `tye.yaml` file that becomes hard to maintain. My advice is that you use tags with care, and consider different `.yaml` files instead to configure different environment.

## Deploying to Kubernetes

First of all, you need to have [Docker](https://docs.docker.com/install/).

You'll need a **container registry** too. In my presentation I used a local registry because I wanted to avoid uploading images to my Docker Hub account, which would take longer, but you can use anything you want, like Azure Container Registry.

Lastly, you'll need a Kubernetes cluster. If you're using Docker Desktop, which you most probably are, the easiest is just to enable Kubernetes there.

> As I said before, don't let the word Kubernetes intimidate you. That's part of the idea behind Tye: to remove that steep learning curve there is today for Kubernetes, so that you can work with it without the need to understand it in detail.

### Using a local container registry

[Run a local registry](https://docs.docker.com/registry/deploying/#run-a-local-registry):

`docker run -d -p 5050:5000 --restart=always --name registry registry:2`

Specify your container registry in `tye.yaml`:

```yaml
name: todoapp
registry: localhost:5050   # Add this line
# ...
```

If you want to use your [Docker Hub account](https://hub.docker.com/), just specify your user name. (In my case, that would be `ignacioerrico`).

Deploying to Kubernetes is as simple as running:

`tye deploy`

Done. It will take a few seconds, but that's really all there is to it.

> As `tye deploy` is running, you'll see messages in red scroll by. Those are not error messages. Those are information messages from Docker. So nothing to worry about.

Let's check the pods (it's okay if you don't know what a _pod_ is):

`kubectl get pods`

Check the services too:

`kubectl get service`

- If you're using Docker Desktop, you'll see the `kubernetes` service in addition to the three services of the application.

Now, this is all fine and dandy, but how can we reach our app now? This is all running in containers, so we need to create a point of ingress into the container of our web app. So run:

`kubectl port-forward svc/todo-web 5001:80`

- That means that, for the service `todo-web` (our web app), we want to forward the HTTP traffic on port 80 (the default for HTTP requests) in the pod that contains that service to the local port 5001. (I picked 5001 at random. Make sure you use a port that is not in use.)

Now navigate to http://localhost:5001/. There's our app, running in Kubernetes!

Let's check our running containers:

`docker container ls`

- Kubernetes is making sure the desired state is met.

Let's be a bit cheeky and mess with Kubernets. Take note of the container ID of one of the services and _kill it_:

`docker rm -f SHA1`

> Replace _SHA1_ by the corresponding container ID.

In an attempt to restore the desired state, Kubernetes will start a new instance of the service you just killed. No matter how hard you try, Kubernetes will do what it takes to make sure the app is running as we specified in our `tye.yaml` file. Users of the app might not even notice there was a glitch.

So if Kubernetes keeps our app running, how do we stop it? You won't get anywhere if you try to stop the containers manually!

With Tye, it's as simple as doing:

`tye undeploy`

### Using Azure

Deploying to Azure is just as simple, which only goes to show that there's a tendency towards eliminating the difference between local and cloud.

I will assume you already have a cluster on Azure Kubernetes Service.

Specify your container registry in `tye.yaml`:

You'll just need to configure a point of ingress:

```yaml
name: todoapp
ingress:
- name: ingress
  bindings:
  - port: 8080
  rules:
  - path: /
    service: todo-web
```

Then, just run:

`tye deploy -i`

> The `-i` modifier stands for _interactive_ and is necessary in this case because Tye will ask us if we want to install the ingress libraries.

Just look for the **IngressIP** in the output, and copy and paste that URL in your browser. There's our containerized app running in the cloud!

To undeploy, yes, you guessed it:

`tye undeploy`

All this just seems too easy, and this is only the beginning…

# Limitations

I said this before, and I say this again: Tye is an experiment. It's in alpha.

**So don't use it in production. Please.**

However, you _can_ use it for development to some extent, and my experience using it has been positive.

I already mentioned some of the current limitations of Tye. An important one to be aware of is that [it won't work with Entity Framework](https://github.com/dotnet/tye/issues/940), which is a common dependency in a project. The problem is that, in order to create a migration, the EF tooling needs to know the connection string, which requires Tye to be running, and that creates a catch-twenty-two kind of issue.

My original idea was to try to make the API persist data to SQL Server. While I could think of workarounds to make that work, I decided it didn't make sense to try to force things if Tye is not ready for that yet, and that it would be more useful to show what _is_ possible.

# Cheat sheet

| Command    | What it does                                                                              |
| ---------- | ----------------------------------------------------------------------------------------- |
| `init`     | Creates a YAML file with default configuration.                                           |
| `run`      | Runs the application either from an inferred config or explicitly from a `tye.yaml` file. |
| `build`    | Containerizes the application.                                                            |
| `push`     | Take the created containers and pushes them to a container registry.                      |
| `deploy`   | Deploys the application to a Kubernetes cluster.                                          |
| `undeploy` | Undeploys from Kubernetes.                                                                |

# Questions?

I'm really passionate about this topic, and I really enjoyed preparing this presetantion.

If you have questions, don't hesitate to [create an issue](https://github.com/ignacioerrico/pw-2021-tye-demo/issues). I'd be happy to discuss it!