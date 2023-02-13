# GiftShop
The first part of our GiftShop Application. This features the use of Code-First Migrations to create our database, and WebAPI and LINQ to perform CRUD operations.
<h2>Running this project</h2>
<ul>
<li> Project > GiftShop Properties > Change target framework to 4.7.1 -> Change back to 4.7.2
<li>Make sure there is an App_Data folder in the project (Right click solution > View in File Explorer)
<li>Tools > Nuget Package Manager > Package Manage Console > Update-Database
<li>Check that the database is created using (View > SQL Server Object Explorer > MSSQLLocalDb > ..)
<li>Run API commands through CURL to create new gifts
<h2>Common Issues and Resolutions</h2>
<li>(update-database) Could not attach .mdf database SOLUTION: Make sure App_Data folder is created
<li>(update-database) Error. 'Type' cannot be null SOLUTION: (issue appears in Visual Studio 2022) Tools > Nuget Package Manager > Manage Nuget Packages for Solution > Install Latest Entity Framework version (eg. 6.4.4), restart visual studio and try again
<li>(update-database) System Exception: Exception has been thrown by the target of an invocation POSSIBLE SOLUTION: Project was cloned to a OneDrive or other restricted cloud-based storage. Clone the project repository to the actual drive on the machine.
<li>(running server) Could not find part to the path ../bin/roslyn/csc.exe SOLUTION: change target framework to 4.7.1 and back to 4.7.2
<li>(running server) Project Failed to build. System.Web.Http does not have reference to serialize... SOLUTION: Solution Explorer > References > Add Reference > System.Web.Extensions
Make sure to utilize jsondata/gift.json to formulate data you wish to send as part of the POST requests. {id} should be replaced with the gifts's primary key ID. The port number may not always be the same

Get a List of Guftss curl https://localhost:44324/api/giftdata/listgiftss

Get a Single Gift curl https://localhost:44324/api/giftdata/findgift/{id}

Add a new Gift (new gift info is in gift.json) curl -H "Content-Type:application/json" -d @gift.json https://localhost:44324/api/giftdata/addgift

Delete a Giftl curl -d "" https://localhost:44324/api/giftdata/deletegift/{id}

Update an Gift (existing gift info including id must be included in gift.json) curl -H "Content-Type:application/json" -d @gift.json https://localhost:44324/api/giftdata/updategift/{id}
<h2>Running the Views for List, Details, New</h2>
<li>Use SQL Server Object Explorer to add a new Order
<li>Take note of the Order ID
<li>Navigate to /Gift/New
<li>Input the GiftBasketSize, GiftBasketQuantity and Order ID
<li>click "Add"
