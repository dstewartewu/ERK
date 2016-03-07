<?php
    require_once 'models/authorize.php';
?>
<!DOCTYPE html>
<html lang="en">
<head>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">

    <title>Career Services Event Management Admin Page</title>

    <!-- Bootstrap Core CSS -->
    <link href="dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- MetisMenu CSS -->
    <link href="dist/js/shared/metisMenu.min.css" rel="stylesheet">
    <!-- Font Awesome Icons -->
    <link href="dist/css/font-awesome.min.css" rel="stylesheet">
    <!-- Custom CSS -->
    <link href="dist/css/sb-admin-2.css" rel="stylesheet">
    <!-- Angularjs Library -->
    <script src="dist/js/shared/angular.min.js"></script>


    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->

</head>

<body ng-app = 'AdminPanel'>

    <div id="wrapper" class="container-fluid col-lg-12 col-md-12">
        <!-- Navigation -->
        <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0; background-color: #a10022">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="#/">Career Services Event Management Admin Panel</a>
            </div>
            <!-- /.navbar-header -->
            <div class="navbar-default sidebar" role="navigation" style="background-color: #ffffff">
                <div class="sidebar-nav navbar-collapse">
                    <ul class="nav" id="side-menu">
                        <li>
                            <img src="images/Logo_Vertical_4-Color.jpg" alt="" id="logo" width="250px">
                        </li>
                        <li>
                            <a href="#/"><i class="fa fa-dashboard fa-fw"></i> Dashboard</a>
                        </li>
                        <li>
                            <a href="#CreateEvent">Create Event</a>
                        </li>
                        <li>
                            <a href="#ManageEvents">Manage Events</a>
                        </li>
                        <li>
                            <a href="#EditQuestions">Edit Questions</a>
                        </li>
                        <li>
                            <a href="#RegisterKiosk">Register Kiosk</a>
                        </li>
                        <li>
                            <a href="#EventReports">Event Reports</a>
                        </li>
                        <li>
                            <a href="#ManageAdmins">Manage Admins</a>
                        </li>
                    </ul>
                    <br/><br/><br/>

                </div>
                <!-- /.sidebar-collapse -->
            </div>
            <!-- /.navbar-static-side -->
        </nav>
        <div id="page-wrapper">
            <div class="container-fluid">

                <!-- content is injected here -->
                <div ng-view>

                </div>

            </div>
            <!-- /container -->
        </div>
    </div>
    <input hidden="hidden" value="QBGDmHa3SFq2Ykp6" id="key">
    <!-- /#wrapper -->
    </div>
    <div class="col-lg-12 col-md-12">
        <br/><br/>
        <p style="width:100%; text-align: center;"><span style="color: white;">
            Eastern Washington University Career Services</br>
            (509)-359-6365</br>
            Careers@ewu.edu</span>
        </p>
    </div>

    <script src="dist/js/shared/angular-route.min.js"></script>
    <!-- jQuery -->
    <script src="dist/js/shared/jquery.min.js"></script>

    <!-- Angular UI Bootstrap Core JavaScript -->
    <script src="dist/js/shared/ui-bootstrap-1.0.3.min.js"></script>
    <script src="dist/js/shared/ui-bootstrap-tpls-1.0.3.min.js"></script>
    <script src="dist/js/shared/bootstrap.min.js"

    <!-- Metis Menu Plugin JavaScript -->
    <script src="dist/js/shared/metisMenu.min.js"></script>

    <!-- Modules -->
    <script src="app.js"></script>

    <!-- Controllers Are All Loaded here for Dynamic Page Switching -->
    <script src="controllers/users.controller.js"></script>
    <script src="controllers/createEvent.controller.js"></script>
    <script src="controllers/dashboard.controller.js"></script>
    <script src="controllers/questionAdd.controller.js"></script>
    <script src="controllers/manageEvent.controller.js"></script>
    <script src="controllers/questionEdit.controller.js"></script>
    <script src="controllers/kiosks.controller.js"></script>
    <script src="controllers/stats.controller.js"></script>

</body>

</html>
