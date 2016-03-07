Career Services Event Management System (Web Side)

Senior Project Group

November 2015 - Met with Danny Musina	
				- Concerns raised 
				
					- Needs CRUD API layer
					- Needs phpCAS 
					- Needs to be compatable with PHP 5.3 and Maria DB
					- Needs to protect against cross-site scripting
					- Dependancy Management - Composer suggested
					- Colors and branding must meet MarCom standards.
					
					All concerns raised have been attempted to be addressed.

3 Parts:

	1. Admin Panel for creating/managing events, creating csv's of event information and creating kiosk tokens
		
		- AngularJS single page web application - views injected into Index.php using AngularJS routes, each view has it's own .js 
		  file to manage all data. Should make issues easy to pin-point in the future.
		- Slim PHP CRUD API using hidden token passed in post routes to certify requests are coming from admin panel
		- phpCAS login called on load. Never leaves index.php so that should be enough to prevent unauthorized access
	
	2. Pre-Registration Site - 3 Pages - 
	
		- Landing Page - Select Event to Pre-Register for.
		
		- Pre-Registration Form
		
		- Pre-Registration Success Page
		
	3. API - 
	
		- Slim PHP API
		
		- Limited to CREATE, UPDATE and READ and limited scope to just the methods needed for the desktop program
		
		- Uses a token to authorize the kiosk for access. Tokens are generated in the Admin Panel and expire 24 hrs
		  after generation.
		  
	Basic Security Measures taken:
	
		- All data is input into the database using parameterized queries.
		
		- All queries from the desktop and pre-reg site are HTML sanitized to prevent cross-site scripting
		
		- Sanitation and parameterization happen on the server side.
		
		- Tokens used to restrict access to API's
		
		- Single Page Web Application used on admin panel so the user must be logged into CAS to access any part of 
		  admin panel
		  
	Needs From OIT-
	
		- Web Hosting for the 3 elements
		
		- A Database
		
		- An email acccount
		
		- All necessary connection information will live in a config.php file located in admin/models
		
		- PHP dependancies are managed using Composer and files live in vendor folder
		
		- Javascript libaries and CSS are stored in the project folders, to prevent issues with remote hosting
		  and provide a stable backing to the websites.
		  