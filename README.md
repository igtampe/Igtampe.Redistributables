![IDACRA Logo](https://raw.githubusercontent.com/igtampe/Igtampe.Redistributables/master/Images/Header%20(Universal).png)
-----
The Igtampe Redistributables (IRED) and Igtampe DBContexts ActionAgents Controllers Reusable Architecture (IDACRA) is a set of class libraries designed to accelerate and ease the process of creating ASP.NET Backends. Using these packages, you can go from 0 to a fully functioning backend in just a few hours, thanks to in-built utilities including a simple User authentication and session management system available right out of box.
<br/>
## What's in the box?
![IDACRA](https://raw.githubusercontent.com/igtampe/Igtampe.Redistributables/master/Images/Diagram%20(Horiz).png)

IRED/IDACRA is comprised of 5 Class Libraries including the following:
- **Redistributables (IRED)**: IRED includes several common objects (Like Chopo Auth and the Chopo Session Manager), and several interfaces
- **Redistributables.DBContexts (IDACRA)**: DBContexts includes the DbContexts portions of IDACRA. It has some interfaces for Actions, and the ever useful PostgresContext configurable DBContext. It's especially useful if you're planning to ship to Heroku, or any other DB that uses Postgres.
- **Redistributables.Actions (IDACRA)**: Actions includes the ActionsAgent portion of IDACRA. It has several reusable actions including UserAgent to retrieve IRED Chopo Users, or Notifier Notifications to send users their notifications
- **Redistributables.ASPControllers (IDACRA)**: ASPControllers includes the Controllers portion of IDACRA. It has controller bases for your new controllers, and reusable ones for IRED entities.
- **Redistributables.Launch (IRED/IDACRA)**: Launch includes a simple ASP.NET Launcher, simplifying the process of getting your API up and running in a single method. Plus, it automatically configures the swagger with the information you provide!

Included is also a Sample Project (Toffee) which shows just how easy it is to create an app with IRED IDACRA. (Frontend still pending)

|||
|-|-|
|![IDACRA](https://raw.githubusercontent.com/igtampe/Igtampe.Redistributables/master/Images/idacra_server.png)|![TOFFEE](https://raw.githubusercontent.com/igtampe/Igtampe.Redistributables/master/Images/toffee_server.png)|
