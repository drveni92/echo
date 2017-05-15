(function () {
    angular
        .module("Billing")
        .controller('LoginController', ['$scope', '$rootScope', '$http', '$location', 'SessionService', 'localStorageService', 'ToasterService', '$timeout', function ($scope, $rootScope, $http, $location, SessionService, localStorageService, ToasterService, $timeout) {
            $http.get("config.json")
                .then(function (response) {
                    BillingConfig = response.data;
                    $rootScope.invoicesCount = 0;
                    var rememberMeToken = localStorageService.cookie.get("Billing");
                    if (rememberMeToken != null) {
                        $http({
                            method: "post",
                            url: BillingConfig.source + "remember",
                            data: {
                                "apiKey": BillingConfig.apiKey,
                                "signature": BillingConfig.signature,
                                "remember": rememberMeToken
                            }
                        }).then(function (response) {
                            credentials = response.data;
                            localStorageService.cookie.set("Billing", credentials.remember, BillingConfig.ExpirationDate);
                            $rootScope.currentUser = credentials.currentUser.name;
                            $rootScope.currentUsername = credentials.currentUser.username;
                            $rootScope.currentUserId = credentials.currentUser.id;
                            redirectTo = (redirectTo == "/logout") ? BillingConfig.DefaultRoute : redirectTo;
                            $location.path(redirectTo);

                            getCountInvoices();

                        }, function (reason) {
                            ToasterService.pop('error', "Error", "Username or password is incorrect");
                        });
                    }
                }, function (reason) {
                    ToasterService.pop('error', "Error", reason);
                });
            $(document).ready(function () {

                $(".content").hide().fadeIn("slow");
                $('#countdown').countdown('2017/05/19 14:00:00', function (event) {
                    $(this).html(event.strftime('%Ddays   %Hh %Mm %Ss'));
                });
            });
            $scope.login = function () {
                $http.defaults.headers.common.Authorization = "Basic " + SessionService.encode($scope.user.name + ":" + $scope.user.pass);
                var promise = $http({
                    method: "post",
                    url: BillingConfig.source + "login",
                    data: {
                        "apiKey": BillingConfig.apiKey,
                        "signature": BillingConfig.signature,
                        "remember": $scope.user.remember
                    }
                });
                promise.then(
                    function (response) {
                        credentials = response.data;
                        $rootScope.currentUser = credentials.currentUser.name;
                        $rootScope.currentUsername = credentials.currentUser.username;
                        $rootScope.currentUserId = credentials.currentUser.id;

                        if ($scope.user.remember) {
                            localStorageService.cookie.set("Billing", credentials.remember, BillingConfig.ExpirationDate);
                        }
                        redirectTo = (redirectTo == "/logout") ? BillingConfig.DefaultRoute : redirectTo;
                        getCountInvoices();
                        $location.path(redirectTo);
                    },
                    function (reason) {
                        document.getElementById('username').style.border = "2px solid #f00";
                        document.getElementById('password').style.border = "2px solid #f00";

                        ToasterService.pop('error', "Error", "Username or password is incorrect");

                        credentials = null;
                    });
            };


            function getCountInvoices() {
                $http.defaults.headers.common.Token = credentials.token;
                $http.defaults.headers.common.ApiKey = BillingConfig.apiKey;

                $http.get(BillingConfig.source + "invoices/automatic/nocheck").then(
                    function (response) {
                        $rootScope.invoicesCount = response.data;
                        $timeout(getCountInvoices, 10000);
                    },
                    function (reason) {
                    }
                );
            };

            $rootScope.getClass = function (path) {
                return ($location.path() === path) ? 'active-menu' : '';
            };

            $('a.page-scroll').bind('click', function(event) {
                var $anchor = $(this);
                $('html, body').stop().animate({
                    scrollTop: ($($anchor.attr('href')).offset().top - 50)
                }, 1250, 'easeInOutExpo');
                event.preventDefault();
            });

            // Highlight the top nav as scrolling occurs
            $('body').scrollspy({
                target: '.navbar-fixed-top',
                offset: 51
            });

            // Closes the Responsive Menu on Menu Item Click
            $('.navbar-collapse ul li a').click(function(){
                $('.navbar-toggle:visible').click();
            });

            // Offset for Main Navigation
            $('#mainNav').affix({
                offset: {
                    top: 100
                }
            });


            $("#contactForm input,#contactForm textarea").jqBootstrapValidation({
                preventSubmit: true,
                submitError: function($form, event, errors) {
                    // additional error messages or events
                },
                submitSuccess: function($form, event) {
                    event.preventDefault(); // prevent default submit behaviour
                    // get values from FORM
                    var name = $("input#name").val();
                    var email = $("input#email").val();
                    var phone = $("input#phone").val();
                    var message = $("textarea#message").val();
                    var firstName = name; // For Success/Failure Message
                    // Check for white space in name for Success/Fail message
                    if (firstName.indexOf(' ') >= 0) {
                        firstName = name.split(' ').slice(0, -1).join(' ');
                    }
                    $.ajax({
                        url: "././mail/contact_me.php",
                        type: "POST",
                        data: {
                            name: name,
                            phone: phone,
                            email: email,
                            message: message
                        },
                        cache: false,
                        success: function() {
                            // Success message
                            $('#success').html("<div class='alert alert-success'>");
                            $('#success > .alert-success').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                                .append("</button>");
                            $('#success > .alert-success')
                                .append("<strong>Your message has been sent. </strong>");
                            $('#success > .alert-success')
                                .append('</div>');

                            //clear all fields
                            $('#contactForm').trigger("reset");
                        },
                        error: function() {
                            // Fail message
                            $('#success').html("<div class='alert alert-danger'>");
                            $('#success > .alert-danger').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                                .append("</button>");
                            $('#success > .alert-danger').append($("<strong>").text("Sorry " + firstName + ", it seems that my mail server is not responding. Please try again later!"));
                            $('#success > .alert-danger').append('</div>');
                            //clear all fields
                            $('#contactForm').trigger("reset");
                        },
                    });
                },
                filter: function() {
                    return $(this).is(":visible");
                },
            });

            $("a[data-toggle=\"tab\"]").click(function(e) {
                e.preventDefault();
                $(this).tab("show");
            });



            /*When clicking on Full hide fail/success boxes */
            $('#name').focus(function() {
                $('#success').html('');
            });



        }])
        .controller('LogoutController', ['$http', 'localStorageService', function ($http, localStorageService) {
            $http({
                method: "get",
                url: BillingConfig.source + "logout",
                async: false
            }).then(function (response) {
                localStorageService.cookie.clearAll("Billing");
                credentials = null;
                window.location.reload();
                return true;
            },
                function (reason) {
                    return false;
                });

        }]);
}());
