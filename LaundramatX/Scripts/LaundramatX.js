/// <reference path="jquery.d.ts" />
/// <reference path="materialize.d.ts" />
//GLOBAL variable to store the name of the shake animation
var animateShake = 'animated shake';
var animateToLeft = 'animated zoomOutLeft';
var animateToRight = 'animated zoomInRight';
var animateVenders = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
//Make use of the Static nav that changesvar  mn = $(".navbar.navbar-default");
var mn = $(".navbar.navbar-default");
var mns = "navbar-fixed-top";
var hdr = $('#header').height();
//The boolean to make sure the RECEIVE FUNCTIONALITY HAPPENS ONLY ONCE
var isin = false;
//Variables that will be used in the profile's page
var Edit_Work_Location = {
    Lat: 0,
    Lon: 0,
    HouseNumber: 'None',
    StreetName: 'None',
    LocalName: 'None',
    City_TownName: 'None',
    Province: 'None',
    Country: 'None'
};
var Edit_Home_Location = {
    Lat: 0,
    Lon: 0,
    HouseNumber: 'None',
    StreetName: 'None',
    LocalName: 'None',
    City_TownName: 'None',
    Province: 'None',
    Country: 'None'
};
var Edit_Company_Location = {
    Lat: 0,
    Lon: 0,
    HouseNumber: 'None',
    StreetName: 'None',
    LocalName: 'None',
    City_TownName: 'None',
    Province: 'None',
    Country: 'None'
};
//The most important variable (Used to map the root).
var rootPath = $('#rootDir').html();
$(window).scroll(function (e) {
    //Make sure we are at the RECIEVE PAGE
    if (($("body[receive]").html() != undefined) && !isin) {
        //This is used by the Receive to go and fetch somemore posts when the user scrolls
        if ($(e.currentTarget).scrollTop() + $(window).height() >= ($(document).height() - $('#FooterX').height() - 200)) {
            $('#LoadingPosts').removeClass('hidden');
            var index = $('.ThePost').toArray().length;
            isin = true;
            $.ajax({
                url: rootPath + 'ReceiveX/getMorePosts?index=' + index,
                contentType: 'json',
                success: function (answer) {
                    var result = JSON.parse(answer);
                    var i = 0;
                    if (result == "No more Posts") {
                        if ($('#toast-container').children().length < 1) {
                            var $ToastMessage = $('<span>No more posts left</span>');
                            Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3');
                        }
                    }
                    else {
                        for (i = 0; i < result.length; i++) {
                            $("#ReceivePosts").append(result[i]);
                        }
                        //Start the masonry
                        $('.grid').imagesLoaded(function () {
                            $('.grid').masonry('reloadItems');
                            $('.grid').masonry('layout');
                        });
                        //Initialize items
                        $('.materialboxed').materialbox();
                        $('.modal-trigger').leanModal();
                        $('.tooltipped').tooltip();
                    }
                    $('#LoadingPosts').addClass('hidden');
                    isin = false;
                }
            });
        }
    }
});
//Glabal variables
var IsloggedIn = false;
var IsChecked = false;
//Maps Maps Maps
function getLatLong() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var lat = position.coords.latitude;
            var lon = position.coords.longitude;
            var city = "";
            var state = "";
            var country = "";
            var geocoder = new google.maps.Geocoder();
            var latlng = new google.maps.LatLng(lat, lon);
            //Geo Stuff
            geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        var result = results[0];
                        var XLocation = {
                            Lat: 0,
                            Lon: 0,
                            HouseNumber: 'None',
                            StreetName: 'None',
                            LocalName: 'None',
                            City_TownName: 'None',
                            Province: 'None',
                            Country: 'None'
                        };
                        XLocation['Lat'] = result['geometry']['viewport']['b']['f'];
                        XLocation['Lon'] = result['geometry']['viewport']['f']['f'];
                        for (var i = result.address_components.length - 1; i >= 0; i--) {
                            switch (result.address_components[i]['types'][0]) {
                                case "country":
                                    XLocation['Country'] = result.address_components[i]['long_name'];
                                    break;
                                case "administrative_area_level_1":
                                    XLocation['Province'] = result.address_components[i]['long_name'];
                                    break;
                                case "locality":
                                    XLocation['City_TownName'] = result.address_components[i]['long_name'];
                                    break;
                                case "sublocality_level_1":
                                    XLocation['LocalName'] = result.address_components[i]['long_name'];
                                    break;
                                case "route":
                                    XLocation['StreetName'] = result.address_components[i]['long_name'];
                                    break;
                                case "street_number":
                                    XLocation['HouseNumber'] = result.address_components[i]['long_name'];
                                    break;
                            }
                        }
                        //Call this major ajax if we have everything
                        $.ajax({
                            method: "POST",
                            url: rootPath + "Account/SetLocation/?lat=" + lat + "&lon=" + lon + "&Country=" + XLocation['Country'] + "&Province=" + XLocation['Province'] + "&City_TownName=" + XLocation['City_TownName'] + "&LocalName=" + XLocation['LocalName'] + "&StreetName=" + XLocation['StreetName'] + "&HouseNumber=" + XLocation['HouseNumber'] + "&Which=current",
                            success: function (result) {
                                if (result == "Done") {
                                    var $ToastMessage = $('<span>Great we got your location ,THANK YOU</span>');
                                    Materialize.toast($ToastMessage, 7000, 'rounded lighten-3 ToastX z-depth-3');
                                }
                                else {
                                    var $ToastMessage = $('<span>Unable to see your location due to some errors  ... ' + result + '</span>');
                                    Materialize.toast($ToastMessage, 7000, 'rounded lighten-3 ToastX z-depth-3');
                                }
                            }
                        });
                        return;
                    }
                    else {
                        var $ToastMessage = $('<span>Unable to see your location , Please log out and log in again</span>');
                        Materialize.toast($ToastMessage, 7000, 'rounded lighten-3 ToastX z-depth-3');
                    }
                }
                else {
                    //Call this mini ajax if we fail to read the state country and city
                    $.ajax({
                        method: "POST",
                        url: rootPath + "Account/SetLocation/?lat=" + lat + "&lon=" + lon + "&Which=current",
                        success: function (result) {
                            if (result === "Done") {
                                var $ToastMessage = $('<span>Great we got your location ,THANK YOU</span>');
                                Materialize.toast($ToastMessage, 7000, 'rounded lighten-3 ToastX z-depth-3');
                            }
                            else {
                                var $ToastMessage = $('<span>Unable to see your location due to some errors  ... ' + result + '</span>');
                                Materialize.toast($ToastMessage, 7000, 'rounded lighten-3 ToastX z-depth-3');
                            }
                        }
                    });
                }
            });
        }, function (error) {
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    var $ToastMessage = $('<div><span style="float:right">You denied to give use your location</span></div>');
                    Materialize.toast($ToastMessage, 7000, 'rounded');
                    break;
                case error.TIMEOUT:
                    var $ToastMessage = $('<div><span style="float:right">Your connect is not that strong</span></div>');
                    Materialize.toast($ToastMessage, 7000, 'rounded');
                    break;
                default:
                    var $ToastMessage = $('<div><span style="float:right">Unable to capture your location</span></div>');
                    Materialize.toast($ToastMessage, 7000, 'rounded');
                    break;
            }
        });
    }
    else {
        var $ToastMessage = $('<div><span style="float:right">Your browser denied us access to your current location</span></div>');
        Materialize.toast($ToastMessage, 7000, 'rounded');
    }
}
function toggleMapDesc(self, lat, lon, DivId) {
    var toggler = $(self).attr('data-Toggled');
    var PostID = $(self).attr('data-PostID');
    if (toggler == 'true') {
        $(self).attr('data-Toggled', 'false');
        $('#DivPostMap_' + PostID).hide('slow');
        $('#DivPostDesc_' + PostID).show('slow');
        $(self).find('span').addClass('black-text');
        $(self).find('span').removeClass('teal-text');
    }
    else {
        $(self).attr('data-Toggled', 'true');
        $('#DivPostMap_' + PostID).show('slow');
        $('#DivPostDesc_' + PostID).hide('slow');
        $(self).find('span').removeClass('black-text');
        $(self).find('span').addClass('teal-text');
        if ($(self).attr('data-MapLoaded') == 'false') {
            getLocation(lat, lon, DivId);
            $(self).attr('data-MapLoaded', 'true');
        }
    }
}
function getLocation(lat, lon, DivId) {
    var latlon = new google.maps.LatLng(lat, lon);
    var myOptions = {
        center: latlon, zoom: 14,
        mapTypeId: google.maps.MapTypeId.MAPROAD,
        mapTypeControl: false,
        navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL }
    };
    var mapholder = document.getElementById(DivId);
    mapholder.style.height = '200px';
    var map = new google.maps.Map(document.getElementById(DivId), myOptions);
    var marker = new google.maps.Marker({ position: latlon, map: map, title: "Work is here" });
}
//Redirect to pages using Url.Action *Most important Method (*_*)
function RedirectAction(link) {
    window.location.href = link;
}
$(window).resize(function () {
    var size = $(window).width();
    if (size <= 838) {
        //Small screen
        $('#HomeIntro').css('font-size', '20px');
    }
    else if (size > 976) {
        //Large screen
        $('#HomeIntro').css('font-size', '40px');
    }
    else {
        //Medium screen
        $('#HomeIntro').css('font-size', '25px');
    }
});
function ContactUsToast(fb, twitter, instagram, GooglePlus) {
    var $ToastMessage = $('<div><span style="float:right">Click on any social network</span><br /><div class=\"input-field\"><a class=\"circle btn btn-floating waves-circle waves-effect waves-yellow XButton XYellowfy\" href="' + fb + '">F</a><a class=\"circle btn btn-floating waves-circle waves-effect waves-yellow XButton XYellowfy\"  href="' + twitter + '">T</a><a class=\"circle btn btn-floating waves-circle waves-effect waves-yellow XButton XYellowfy\" href="' + GooglePlus + '">G+</a><a class=\"circle btn btn-floating waves-circle waves-effect waves-yellow XButton XYellowfy\" href="' + instagram + '">I</a></div><br /></div>');
    Materialize.toast($ToastMessage, 10000, 'rounded');
}
window.setInterval(function () {
    //Check if the user is logged in
    if (!IsChecked) {
        if (localStorage.getItem("IsloggedIn") != null) {
            if (localStorage.getItem("IsloggedIn") == "true" && localStorage.getItem("IsChecked") == "false") {
                getLatLong();
                localStorage.setItem("IsChecked", "true");
                IsChecked = true;
            }
        }
    }
    CheckNotifications();
}, 5000);
$(document).ready(function () {
    //FIRST THING WE CHECK FOR NOTIFICATIONS
    CheckNotifications();
    //Init The croppie to crop images
    var opts = "";
    $('#EditProfilePicImg').croppie(opts);
    //Hover animations for the details columns
    $('#UlAboutX .list').mouseenter(function (e) {
        $(e.currentTarget).addClass("z-depth-5");
    });
    $('#UlAboutX .list').mouseleave(function (e) {
        $(e.currentTarget).removeClass("z-depth-5");
    });
    //The fire scroll functionality on the home page
    var options = [{
            selector: '#AboutX', offset: 400, callback: function (e) {
                Materialize.showStaggeredList($(e));
            }
        },
        {
            selector: '#PricingX', offset: 400, callback: function (e) {
                Materialize.showStaggeredList($(e));
            }
        }];
    Materialize.scrollFire(options);
    //Start the method that Resizes the windows
    $(window).resize();
    //Scroll the window
    $(window).scroll();
    //Start the Collaspible for the Stuff Page
    $('.collapsible').collapsible();
    //Start the large image thingi
    $('.materialboxed').materialbox();
    //Start the Carousel that has fixed width
    $('.carouselX').carousel({});
    $('.carousel').carousel({ indicators: true, full_width: true });
    $('.carousel-slider').slider({ indicators: true });
    //Start the Select on options in a way of radio buttons
    $('select').material_select();
    //Start the counter of inputting contact numbers
    $('input#num').characterCounter();
    //Start The SideNav functionality
    $(".button-collapse").sideNav();
    //Start The dropdown functionality
    $('.dropdown-button').dropdown({
        belowOrigin: false,
        gutter: 1000
    });
    //Start the Modal functionality
    $(".modal-trigger").leanModal({
        dismissible: false,
        opacity: .10
    });
    //Start the masonry
    $('.grid').imagesLoaded(function () {
        $('.grid').masonry({
            itemSelector: ".blog",
            columnWidth: ".blog-sizer"
        });
    });
    //Start the date picker
    $('.datepicker').pickadate({
        selectMonths: true,
        selectYears: 15 // Creates a dropdown of 15 years to control year
    });
    //Start the time picker
    $('.timepicker').pickatime({
        twelvehour: false,
        donetext: 'Set',
        autoclose: false,
        vibrate: true
    });
    //Start the tooltip
    $('.tooltipped').tooltip({ delay: 50 });
    //Start the search functionality
    $(".inpt_search").on('focus', function (e) {
        $(e.currentTarget).parent('label').addClass('active');
    });
    $(".inpt_search").on('blur', function (e) {
        if ($(e.currentTarget).val().length == 0)
            $(e.currentTarget).parent('label').removeClass('active');
    });
    //Animate the receive and send fabs to help them grow and change colour when hovered
    $('.XButton').mouseenter(function (e) {
        $(e.currentTarget).addClass("btn-large");
        $(e.currentTarget).addClass("RedBg");
    });
    $('.XButton').mouseleave(function (e) {
        $(e.currentTarget).removeClass("btn-large");
    });
    //The XButton that does the opposite of the XButton *Makes sense?? lol
    $('.XButtonX').mouseleave(function (e) {
        $(e.currentTarget).addClass("wave-effect");
        $(e.currentTarget).addClass("btn-large");
    });
    $('.XButtonX').mouseenter(function (e) {
        $(e.currentTarget).removeClass("btn-large");
    });
    //change the background yellow colour to be bright when we mouseenter the component
    $('.XYellowfy').mouseenter(function (e) {
        $(e.currentTarget).removeClass('lighten-3');
    });
    $('.XYellowfy').mouseleave(function (e) {
        $(e.currentTarget).addClass('lighten-3');
    });
    //Deal with FORM VALIDATION of the Register Page 
    if (($("body[register]").html() != undefined)) {
        //Name Validation
        $('#RegName').keyup(function (e) {
            var Name = $(e.currentTarget).val();
            //Check if the name is valid and meets the length required
            if (Name.length < 3 || (Name.search("^[A-Za-z]+$") === -1)) {
                if (Name.length == 0) {
                    $('#NameError').html("This is required");
                }
                else if (Name.search("^[A-Za-z]+$") === -1) {
                    $('#NameError').html("Fill in a valid name");
                }
                else {
                    $('#NameError').html("The name must be more than 2 charactors long");
                }
                $(e.currentTarget).addClass("incorrect");
                $('#NameError').addClass("red-text");
                $('#NameError').removeClass("green-text");
                Validate[0] = false;
                return;
            }
            else {
                //The correct name is entered
                $(e.currentTarget).removeClass("incorrect");
                $('#NameError').removeClass("red-text");
                $('#NameError').addClass("green-text");
                $('#NameError').html("correct");
                Validate[0] = true;
            }
        });
        //Surname Validation
        $('#RegSurname').keyup(function (e) {
            var Surname = $(e.currentTarget).val();
            //Check if the name is valid and meets the length required
            if (Surname.length < 3 || (Surname.search("^[A-Za-z]+$") === -1)) {
                if (Surname.length == 0) {
                    $('#SurnameError').html("This is required");
                }
                else if (Surname.search("^[A-Za-z]+$") === -1) {
                    $('#SurnameError').html("Fill in a valid surname");
                }
                else {
                    $('#SurnameError').html("The surname must be more than 2 charactors long");
                }
                $(e.currentTarget).addClass("incorrect");
                $('#SurnameError').addClass("red-text");
                $('#SurnameError').removeClass("green-text");
                Validate[1] = false;
                return;
            }
            else {
                //The correct name is entered
                $(e.currentTarget).removeClass("incorrect");
                $('#SurnameError').removeClass("red-text");
                $('#SurnameError').addClass("green-text");
                $('#SurnameError').html("correct");
                Validate[1] = true;
            }
        });
        //Contact Validation
        $('#RegContact').keyup(function (e) {
            var Contact = $(e.currentTarget).val();
            var number = +Contact;
            //Check if the contact is valid and meets the length required
            if (number.toString().length < 9 || Contact.length == 0 || (Contact.search("^[0-9]{10}$") === -1) || (Contact.charAt(0).search("^[0]$") === -1)) {
                if (Contact.length == 0) {
                    $('#ContactError').html("This is required");
                }
                else {
                    $('#ContactError').html("Fill in valid SA Contact Numbers (10 digits)");
                }
                $(e.currentTarget).addClass("incorrect");
                $('#ContactError').addClass("red-text");
                $('#ContactError').removeClass("green-text");
                Validate[2] = false;
                return;
            }
            else {
                //The correct contact is entered
                $(e.currentTarget).removeClass("incorrect");
                $('#ContactError').removeClass("red-text");
                $('#ContactError').addClass("green-text");
                $('#ContactError').html("correct");
                Validate[2] = true;
            }
        });
        //Password Validation
        $('#RegPassword').keyup(function (e) {
            var Password = $(e.currentTarget).val();
            //Check if the password is valid and meets the length required
            if (Password.length < 5) {
                if (Password.length == 0) {
                    $('#PasswordError').html("This is required");
                }
                else {
                    $('#PasswordError').html("The password must be more than 4 charactors long");
                }
                $(e.currentTarget).addClass("incorrect");
                $('#PasswordError').addClass("red-text");
                $('#PasswordError').removeClass("green-text");
                Validate[3] = false;
                return;
            }
            else {
                //The correct name is entered
                $(e.currentTarget).removeClass("incorrect");
                $('#PasswordError').removeClass("red-text");
                $('#PasswordError').addClass("green-text");
                $('#PasswordError').html("correct");
                Validate[3] = true;
            }
        });
        //ConfirmPassword Validation
        $('#RegConfirmPassword').keyup(function (e) {
            var Password = $(e.currentTarget).val();
            //Check if the password is valid and meets the length required
            if (Password.length < 5 || !($('#RegPassword').val() === $('#RegConfirmPassword').val())) {
                if (Password.length == 0) {
                    $('#ConfirmPasswordError').html("This is required");
                }
                else if (Password.length < 5) {
                    $('#ConfirmPasswordError').html("The password must be more than 4 charactors long");
                }
                else {
                    $('#ConfirmPasswordError').html("Your passwords must match");
                }
                $(e.currentTarget).addClass("incorrect");
                $('#ConfirmPasswordError').addClass("red-text");
                $('#ConfirmPasswordError').removeClass("green-text");
                Validate[7] = false;
                return;
            }
            else {
                //The correct passwords is entered
                $(e.currentTarget).removeClass("incorrect");
                $('#ConfirmPasswordError').removeClass("red-text");
                $('#ConfirmPasswordError').addClass("green-text");
                $('#ConfirmPasswordError').html("correct");
                Validate[7] = true;
            }
        });
        //Email Validation
        $('#RegEmail').keyup(function (e) {
            var Email = $(e.currentTarget).val();
            //Check if the email is valid and meets the length required
            if (Email.length === 0 || (Email.search("^\\S+@\\S+$") === -1)) {
                if (Email.length === 0) {
                    $('#EmailError').html("This is required");
                }
                else {
                    $('#EmailError').html("Fill in a valid email address");
                }
                $(e.currentTarget).addClass("incorrect");
                $('#EmailError').addClass("red-text");
                $('#EmailError').removeClass("green-text");
                Validate[4] = false;
                return;
            }
            else {
                //The correct email is entered
                $(e.currentTarget).removeClass("incorrect");
                $('#EmailError').removeClass("red-text");
                $('#EmailError').addClass("green-text");
                $('#EmailError').html("correct");
                Validate[4] = true;
            }
        });
    }
    /*******END REGISTRATION'S PAGE LOAD*******************/
    /*********************** PROFILE'S PAGE *****************/
    if ($("body[Profile]").html() != undefined) {
        //I use this to display the image right after the user clicks it on image edit
        $('#EditProfilePic').change(function (e) {
            $('#ChangeProfileLoad').removeClass('hidden');
            $('#SetProfilePic').addClass('disabled');
            function ReadImage(input) {
                var reader = new FileReader();
                reader.onload = function (es) {
                    $('#EditProfilePicImg').attr('src', es.target.result);
                    $('#ChangeProfileLoad').addClass('hidden');
                    $('#SetProfilePic').removeClass('disabled');
                    var data = es.target.result.toString().split(',')[1];
                    $.post('/SendX/GetImage', { 'image': data }, function (result) {
                        console.log(result);
                    });
                };
                reader.readAsDataURL(input.files[0]);
            }
            ReadImage(this);
        });
        var UserID = +$('head').attr('data-UserID');
        if (isNaN(UserID)) {
            window.location.href = '/home/index';
        }
        $.ajax({
            url: rootPath + 'SendX/GetUserLocations?UserID=' + UserID,
            success: function (answer) {
                var result = JSON.parse(answer);
                if (result[0] != "No Location") {
                    result[0] = JSON.parse(result[0]);
                    $('#Edit_HomeLocationText_Old').html(ConvertCollectAt(result[0]));
                }
                else {
                    $('#Edit_HomeLocationText_Old').html(ConvertCollectAt(null));
                }
                if (result[1] != "No Location") {
                    result[1] = JSON.parse(result[1]);
                    $('#Edit_WorkLocationText_Old').html(ConvertCollectAt(result[1]));
                }
                else {
                    $('#Edit_WorkLocationText_Old').html(ConvertCollectAt(null));
                }
            }
        });
        $("#EditCampanyAddress").geocomplete({})
            .bind("geocode:result", function (event, result) {
            Edit_Company_Location = {
                Lat: 0,
                Lon: 0,
                HouseNumber: 'None',
                StreetName: 'None',
                LocalName: 'None',
                City_TownName: 'None',
                Province: 'None',
                Country: 'None'
            };
            Edit_Company_Location['Lat'] = result['geometry']['viewport']['b']['f'];
            Edit_Company_Location['Lon'] = result['geometry']['viewport']['f']['f'];
            for (var i = result.address_components.length - 1; i >= 0; i--) {
                switch (result.address_components[i]['types'][0]) {
                    case "country":
                        Edit_Company_Location['Country'] = result.address_components[i]['long_name'];
                        break;
                    case "administrative_area_level_1":
                        Edit_Company_Location['Province'] = result.address_components[i]['long_name'];
                        break;
                    case "locality":
                        Edit_Company_Location['City_TownName'] = result.address_components[i]['long_name'];
                        break;
                    case "sublocality_level_1":
                        Edit_Company_Location['LocalName'] = result.address_components[i]['long_name'];
                        break;
                    case "route":
                        Edit_Company_Location['StreetName'] = result.address_components[i]['long_name'];
                        break;
                    case "street_number":
                        Edit_Company_Location['HouseNumber'] = result.address_components[i]['long_name'];
                        break;
                }
            }
        })
            .bind("geocode:error", function (event, status) {
            alert("Map error");
        });
        $("#Edit_HomeLocationText").geocomplete({ map: '#Edit_HomeLocationMap' })
            .bind("geocode:result", function (event, result) {
            Edit_Home_Location = {
                Lat: 0,
                Lon: 0,
                HouseNumber: 'None',
                StreetName: 'None',
                LocalName: 'None',
                City_TownName: 'None',
                Province: 'None',
                Country: 'None'
            };
            Edit_Home_Location['Lat'] = result['geometry']['viewport']['b']['f'];
            Edit_Home_Location['Lon'] = result['geometry']['viewport']['f']['f'];
            for (var i = result.address_components.length - 1; i >= 0; i--) {
                switch (result.address_components[i]['types'][0]) {
                    case "country":
                        Edit_Home_Location['Country'] = result.address_components[i]['long_name'];
                        break;
                    case "administrative_area_level_1":
                        Edit_Home_Location['Province'] = result.address_components[i]['long_name'];
                        break;
                    case "locality":
                        Edit_Home_Location['City_TownName'] = result.address_components[i]['long_name'];
                        break;
                    case "sublocality_level_1":
                        Edit_Home_Location['LocalName'] = result.address_components[i]['long_name'];
                        break;
                    case "route":
                        Edit_Home_Location['StreetName'] = result.address_components[i]['long_name'];
                        break;
                    case "street_number":
                        Edit_Home_Location['HouseNumber'] = result.address_components[i]['long_name'];
                        break;
                }
            }
            $("#Edit_HomeLocationText_New").html(ConvertCollectAt(Edit_Home_Location));
            $('#Edit_HomeLocationBtn_SaveChanges').removeClass('disabled');
        })
            .bind("geocode:error", function (event, status) {
            alert("Map error");
        });
        $("#Edit_WorkLocationText").geocomplete({ map: '#Edit_WorkLocationMap' })
            .bind("geocode:result", function (event, result) {
            Edit_Work_Location = {
                Lat: 0,
                Lon: 0,
                HouseNumber: 'None',
                StreetName: 'None',
                LocalName: 'None',
                City_TownName: 'None',
                Province: 'None',
                Country: 'None'
            };
            Edit_Work_Location['Lat'] = result['geometry']['viewport']['b']['f'];
            Edit_Work_Location['Lon'] = result['geometry']['viewport']['f']['f'];
            for (var i = result.address_components.length - 1; i >= 0; i--) {
                switch (result.address_components[i]['types'][0]) {
                    case "country":
                        Edit_Work_Location['Country'] = result.address_components[i]['long_name'];
                        break;
                    case "administrative_area_level_1":
                        Edit_Work_Location['Province'] = result.address_components[i]['long_name'];
                        break;
                    case "locality":
                        Edit_Work_Location['City_TownName'] = result.address_components[i]['long_name'];
                        break;
                    case "sublocality_level_1":
                        Edit_Work_Location['LocalName'] = result.address_components[i]['long_name'];
                        break;
                    case "route":
                        Edit_Work_Location['StreetName'] = result.address_components[i]['long_name'];
                        break;
                    case "street_number":
                        Edit_Work_Location['HouseNumber'] = result.address_components[i]['long_name'];
                        break;
                }
            }
            $("#Edit_WorkLocationText_New").html(ConvertCollectAt(Edit_Work_Location));
            $('#Edit_WorkLocationBtn_SaveChanges').removeClass('disabled');
        })
            .bind("geocode:error", function (event, status) {
            alert("Map error");
        });
    }
    /**************** END PROFILE'S PAGE **********************/
    //This is everything that happens on the NOTIFICATION'S page
    if (($("body[notifications]").html() != undefined)) {
        var UserID = +$("head").attr("data-UserID");
        if (!isNaN(UserID)) {
            $.ajax({
                method: "POST",
                url: rootPath + 'Notify/GetNotifications?UserID=' + UserID + '&All=' + true + '&inDetail=' + true,
                success: function (result) {
                    var notifications = JSON.parse(result);
                    $('#notifications').html(notifications);
                }
            });
        }
    }
    /*******END NOTIFICATION'S PAGE LOAD*******************/
    /****************SEND'S PAGE STUFF***********************/
    if ($("body[send]").html() != undefined) {
        //Send items chip functionallity
        //During each input in the input area
        $('#SendChipList').keyup(function (e) {
            $('#SendChipsErrors').html("");
            var s = $('#SendChipList').val();
            var last = s.charAt(s.length - 1);
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if ((last == " " || last == "," || keycode == 13)) {
                AppendChips();
            }
            if ($('#SendChips').children().length < 2) {
                $('#SendChipsErrors').html("Please enter atleast two clothing items");
            }
        });
        function AppendChips() {
            var s = $('#SendChipList').val();
            if (s.length < 2) {
                //The person didn't enter anything
                return;
            }
            $('#SendChips').append('<div class="chip"><input class="BasketChip" value="2" max="100" min="1" type="number" /><span>' + s + '</span><i class="close material-icons">close</i></div>');
            $('#SendChipList').val("");
        }
        $('#BtnChipAdd').click(function (e) {
            AppendChips();
        });
        //After the user clicks 'Next 1'
        $('#SendChipsBtn').click(function (e) {
            $('#SendChipsErrors').html("");
            //Make sure that the user entered atleast 2 clothing items
            if ($('#SendChips').children().length < 2) {
                $('#SendChipsErrors').html("Please enter atleast two clothing items");
                //Shake the button 
                $(e.currentTarget).addClass(animateShake).one(animateVenders, function (es) {
                    $(e.currentTarget).removeClass(animateShake);
                });
                return;
            }
            //Make sure that the user entered a value for each chip
            $('#SendChips').children().each(function (index, e) {
                var item = $(e).find('span').html();
                var quantity = $(e).find('input').val();
                if (quantity <= 0 || quantity >= 100 || isNaN(quantity)) {
                    $('#SendChipsErrors').html("You can't send " + quantity + " " + item + "s");
                    return;
                }
                SendItemNames[+index] = item;
                SendItemQuantity[+index] = quantity;
            });
            if ($('#SendChipsErrors').html().length > 1) {
                //Shake the button 
                $(e.currentTarget).addClass(animateShake).one(animateVenders, function (es) {
                    $(e.currentTarget).removeClass(animateShake);
                });
                return;
            }
            AnimateDiv('#SendChipsDiv', '#SendChipsSelection');
        });
        //When a user clicks next after making all the send selections
        $('#SendChipsBtnSelection').click(function (e) {
            properties = [];
            if ($('#WhereToWashL').prop('checked')) {
                //IF the user requires thier laundry to be done at the laundromat
                $('.LaundryProperty').each(function (i, e) {
                    if ($(e).prop('checked')) {
                        properties.push($(e).parent().find('label').html());
                    }
                });
            }
            else {
                //The user wants their laundry to be done at thier place
                $('.HomeProperty').each(function (i, e) {
                    if ($(e).prop('checked')) {
                        properties.push($(e).parent().find('label').html());
                    }
                });
            }
            AnimateDiv('#SendChipsSelection', '#SendChips2Div');
        });
        //When a user clicks previous to go to send selections
        $('#SendChipsBtnSelectionPrv').click(function (e) {
            //TODO : Compare with upper method
            AnimateDiv('#SendChipsSelection', '#SendChipsDiv', true);
        });
        //Move from final div to second last
        $('#SendClothesSubmitPrv').click(function (e) {
            AnimateDiv('#SendChips2Div', '#SendChipsSelection', true);
        });
        function SendPriceChange() {
            var price = +$('#SendPrice').val();
            if (price.toString().length >= 4 || price <= 0 || isNaN(price)) {
                $('#SendPrice').addClass('incorrect');
                $('#SendPriceError').html("Can't take a price of " + price);
                $('#SendPriceIcon').addClass('red-text');
            }
            else {
                $('#SendPrice').removeClass('incorrect');
                $('#SendPriceError').html("");
                $('#SendPriceIcon').removeClass('red-text');
            }
        }
        function SendCollectPlaceChange(that) {
            $('#CollectAt_Selected_Text').html(ConvertCollectAt(null));
            if ($(that).attr('id') == 'CollectAt_Custom') {
                $('#CollectAt_Selected_Text').html($('#CollectAt_Custom_Selected').html());
            }
            else if ($(that).attr('id') == 'CollectAt_Home') {
                $('#CollectAt_Selected_Text').html($('#CollectAt_Home_Selected').html());
            }
            else if ($(that).attr('id') == 'CollectAt_Work') {
                $('#CollectAt_Selected_Text').html($('#CollectAt_Work_Selected').html());
            }
            if ($('#CollectAt_Selected_Text').html() == ConvertCollectAt(null)) {
                $('#SendPriceDiv').hide('slide-left');
                $('#MiniDescriptionDiv').hide('slide-left');
            }
            else {
                $('#SendPriceDiv').show('slide-left');
                $('#MiniDescriptionDiv').show('slide-left');
            }
            //$('#MiniDescriptionDiv').hide('slide-left');
            //$('#SendPriceDiv').hide('slide-left');
        }
        var XLocation = {
            Lat: 0,
            Lon: 0,
            HouseNumber: 'None',
            StreetName: 'None',
            LocalName: 'None',
            City_TownName: 'None',
            Province: 'None',
            Country: 'None'
        };
        function SendDateTime() {
            $('#SendDateTimeError').html("");
            var date = $('#SendDate').val();
            var time = $('#SendTime').val();
            if (date.length < 2) {
                $('#WhereToCollectDiv').hide('slide-left');
                $('#SendDateTimeError').html("Please choose a valid due date");
                $('#SendDate').addClass('incorrect');
                return;
            }
            else if (time.length < 2) {
                $('#WhereToCollectDiv').hide('slide-left');
                $('#SendTime').addClass('incorrect');
                $('#SendDateTimeError').html("Please choose a valid due time");
                return;
            }
            else {
                $('#WhereToCollectDiv').show('slide-left');
                $('#SendTime').removeClass('incorrect');
                $('#SendDate').removeClass('incorrect');
                $('#SendDateTimeError').html("");
                var UserID = +$("head").attr("data-UserID");
                $('#CollectAtDiv').hide('slow');
                $('#CollectAtLoading').show('fast');
                $.ajax({
                    url: rootPath + 'SendX/GetUserLocations?UserID=' + UserID,
                    success: function (answer) {
                        var result = JSON.parse(answer);
                        if (result[0] != "No Location") {
                            result[0] = JSON.parse(result[0]);
                            $('#CollectAt_Home').addClass('BtnPlaceSelectionDiv');
                            $('#CollectAt_Home_Selected').html(ConvertCollectAt(result[0]));
                        }
                        else {
                            $('#CollectAt_Home').removeClass('BtnPlaceSelectionDiv');
                        }
                        if (result[1] != "No Location") {
                            result[1] = JSON.parse(result[1]);
                            $('#CollectAt_Work').addClass('BtnPlaceSelectionDiv');
                            $('#CollectAt_Work_Selected').html(ConvertCollectAt(result[1]));
                        }
                        else {
                            $('#CollectAt_Work').removeClass('BtnPlaceSelectionDiv');
                        }
                        if (result[2] != "No Location") {
                            result[2] = JSON.parse(result[2]);
                            $('#CollectAt_Selected_Text').html(ConvertCollectAt(result[2]));
                            $('#SendPriceDiv').show('slide-left');
                            $('#MiniDescriptionDiv').show('slide-left');
                        }
                        $('#CollectAt_Custom_Selected').html(ConvertCollectAt(null));
                        //Bind the search to autocomplete and the map
                        var options = {
                            map: "#CollectAt_Custom_Map"
                        };
                        $("#CollectAt_Custom_Search").geocomplete(options)
                            .bind("geocode:result", function (event, result) {
                            XLocation = {
                                Lat: 0,
                                Lon: 0,
                                HouseNumber: 'None',
                                StreetName: 'None',
                                LocalName: 'None',
                                City_TownName: 'None',
                                Province: 'None',
                                Country: 'None'
                            };
                            XLocation['Lat'] = result['geometry']['viewport']['b']['f'];
                            XLocation['Lon'] = result['geometry']['viewport']['f']['f'];
                            for (var i = result.address_components.length - 1; i >= 0; i--) {
                                switch (result.address_components[i]['types'][0]) {
                                    case "country":
                                        XLocation['Country'] = result.address_components[i]['long_name'];
                                        break;
                                    case "administrative_area_level_1":
                                        XLocation['Province'] = result.address_components[i]['long_name'];
                                        break;
                                    case "locality":
                                        XLocation['City_TownName'] = result.address_components[i]['long_name'];
                                        break;
                                    case "sublocality_level_1":
                                        XLocation['LocalName'] = result.address_components[i]['long_name'];
                                        break;
                                    case "route":
                                        XLocation['StreetName'] = result.address_components[i]['long_name'];
                                        break;
                                    case "street_number":
                                        XLocation['HouseNumber'] = result.address_components[i]['long_name'];
                                        break;
                                }
                            }
                            $('#CollectAt_Custom_Selected').html(ConvertCollectAt(XLocation));
                            SendCollectPlaceChange($('#CollectAt_Custom'));
                        })
                            .bind("geocode:error", function (event, status) {
                            $('#CollectAt_Custom_Selected').html(ConvertCollectAt(null));
                            $('#CollectAt_Custom_Text').html('ERROR ' + status).addClass('red-text');
                        });
                        $('#CollectAtDiv').show('slow');
                        $('#CollectAtLoading').hide('fast');
                    }
                });
            }
        }
        $('#SendDate').on('change', function (e) {
            SendDateTime();
        });
        $('#SendTime').on('change', function (e) {
            SendDateTime();
        });
        $('#SendPrice').keyup(function (e) {
            SendPriceChange();
        });
        $('#SendPrice').on('change', function (e) {
            SendPriceChange();
        });
        $('.BtnPlaceSelectionDiv').on('click', function (e) {
            if ($(e.currentTarget).hasClass('BtnPlaceSelectionDiv')) {
                if (!$(e.currentTarget).hasClass('z-depth-5')) {
                    $('.BtnPlaceSelectionDiv').removeClass('z-depth-5 grey lighten-3');
                    $(e.currentTarget).addClass('z-depth-5 grey lighten-3');
                    SendCollectPlaceChange($(e.currentTarget));
                }
            }
        });
    }
    /****************END SEND'S PAGE STUFF***********************/
    //LOG IN STUFF//
    //When the user clicks enter on the password field
    $('#LogInPassword').keyup(function (e) {
        //Add brightness to the icon when password is entered
        var password = $('#LogInPassword').val();
        if (password.length > 0) {
            $('#LogInPasswordIcon').addClass("teal-text");
        }
        else {
            $('#LogInPasswordIcon').removeClass("teal-text");
        }
        if (e.which == 13) {
            $('.LogInSubmit').click();
        }
    });
    //When the user clicks on enter in on the contact's field
    $('#LogInContacts').keyup(function (e) {
        var contact = +$('#LogInContacts').val();
        if (contact.toString().length == 9) {
            $('#LogInContactIcon').addClass("teal-text");
        }
        else {
            $('#LogInContactIcon').removeClass("teal-text");
        }
        if (e.which == 13) {
            $('.LogInSubmit').click();
        }
    });
    //When the user is searching
    $('.inpt_search').keyup(function (e) {
        var search = $(e.currentTarget).val();
        if (e.which == 13) {
            SearchFor(e.currentTarget);
        }
        //Hide and show the Search options
        if (search.length >= 1) {
            $('#SearchOptions').removeClass('hidden');
            $('#SearchSubmit').show('slow');
        }
        else {
            $('#SearchSubmit').hide('slow');
            $('#SearchOptions').addClass('hidden');
        }
    });
    $('#SearchReceive').keyup(function (e) {
        var search = $('#SearchReceive').val();
        if (search.length < 2) {
            $("#ReceivePosts").show('slow');
            $("#SearchResults").hide('slow');
            $('#ReceiveSearchLoading').hide('slow');
            $('.grid').imagesLoaded(function () {
                $('.grid').masonry('reloadItems');
                $('.grid').masonry('layout');
            });
            return;
        }
        $('#ReceiveSearchLoading').show('slow');
        var link = $('#SearchReceive').attr('link');
        $.ajax({
            url: link + "?search=" + search,
            contentType: 'json',
            success: function (answer) {
                var result = JSON.parse(answer);
                var i = 0;
                if (result == "No more Posts") {
                    if ($('#toast-container').children().length < 1) {
                        var $ToastMessage = $('<span>No results found</span>');
                        Materialize.toast($ToastMessage, 4000, 'rounded ToastX');
                    }
                    $("#ReceivePosts").show('slow');
                    $("#SearchResults").hide('slow');
                }
                else {
                    $("#ReceivePosts").hide('slow');
                    $("#SearchResults").show('slow');
                    $("#SearchResults").html('<div class="blog-sizer"></div>' + result);
                    $("#SearchResults").children().find('.product-card').addClass('z-depth-5').css('color', 'red');
                }
                $('#ReceiveSearchLoading').hide('slow');
                //Initialize items
                $('.materialboxed').materialbox();
                $('.modal-trigger').leanModal();
                $('.tooltipped').tooltip();
                $('.grid').imagesLoaded(function () {
                    $('.grid').masonry('reloadItems');
                    $('.grid').masonry('layout');
                });
            }
        });
    });
    //perform the search after the user hits enter
    $('#SearchText').keyup(function (e) {
        if (e.which == 13) {
            PerformTheSearch('#SearchText', '#SearchFilter', '#SearchSort', $(e.currentTarget).attr('link'));
        }
    });
    $('._hoverable').mouseenter(function (e) {
        $(e.currentTarget).addClass('z-depth-4');
    });
    $('._hoverable').mouseleave(function (e) {
        $(e.currentTarget).removeClass('z-depth-4');
    });
    //Searching for clothing from the chips
    $('#SearchClothesChip').keyup(function (e) {
        var search = $(e.currentTarget).val();
        if (search.length < 1) {
            $('.SuggestionTag').show('slow');
            return;
        }
        $('.SuggestionTag').each(function (index, element) {
            var text = $(this).find('span').html();
            if (text.toLowerCase().indexOf(search.toLowerCase()) >= 0) {
                $(this).show('slow');
                //We found
            }
            else {
                //Hide the rest
                $(this).hide('slow');
            }
        });
    });
    //Deal with the Where to do laundry show and hide
    //This is the Account view page
    if ($('.ProfileAccountPage').html() != undefined) {
        var userid = +$('.ProfileAccountPage').html();
        GetAllStars(userid);
        $('#BtnToggle_Index_Work').click(function (e) {
            if ($('#ProfileIndex_Profile').hasClass('visible')) {
                $('#ProfileIndex_Profile').show('slide-left');
                $('#ProfileIndex_Work').hide('slide-right');
                $('#ProfileIndex_Profile').removeClass('visible');
            }
            else {
                $('#ProfileIndex_Profile').hide('slide-left');
                $('#ProfileIndex_Work').show('slide-right');
                $('#ProfileIndex_Profile').addClass('visible');
            }
        });
        //Timeline functionality
        $('.WorkLI').click(function (e) {
            if ($(e.currentTarget).hasClass('complete')) {
                return;
            }
            var OrderID = +$(e.currentTarget).attr('data-OrderID');
            var parent = "#" + $(e.currentTarget).parent().attr('id');
            var greater = false;
            var ongoing = true;
            $(parent + ' .WorkLI').each(function (i, el) {
                if (greater) {
                    $(el).removeClass('complete');
                    $(el).find('.date').html('....');
                    if (ongoing) {
                        $(el).find('.date').html('On-going');
                        ongoing = false;
                    }
                }
                else {
                    $(el).addClass('complete');
                }
                if ($(e.currentTarget).html() == $(el).html()) {
                    $(el).addClass('complete');
                    greater = true;
                }
            });
            var helpID = +$(parent).attr('data-helpID');
            $.ajax({
                url: rootPath + 'Notify/MoveStatus?PostHelpID=' + helpID + '&OrderID=' + OrderID,
                success: function (result) {
                    alert(result);
                }
            });
        });
    }
    //This is the send Page
    if ($("body[send]").html() != undefined) {
        var value = $('#EstimatedSendPrice').html();
        $('#EstimatedSendPrice').html("10000");
        $('.WhatService').on('change', function (e) {
            if ($(e.currentTarget).prop('checked')) {
                $('#SendClassified').prop('checked', false);
                TriggerClassified($("#SendClassified"));
                var id = $(e.currentTarget).attr('Id');
                if (id == "WhatServiceL") {
                    //The user wants their laundry to be done
                    $('.DrycleanProperties').hide('slide-left');
                    $('.LaundryProperties').show('slide-right');
                }
                else {
                    //The user wants their dry clean to be done
                    $('.DrycleanProperties').show('slide-left');
                    $('.LaundryProperties').hide('slide-right');
                }
            }
        });
        //Deal with classified
        $('#SendClassified').on('change', function (e) {
            TriggerClassified(e);
        });
        $('.WhereToWash').on('change', function (e) {
            if ($(e.currentTarget).prop('checked')) {
                var id = $(e.currentTarget).attr('Id');
                if (id == "WhereToWashL") {
                    //The user wants their laundry done at the laundry mart
                    $('.HomeProperties p input').attr('disabled', "disabled");
                    $('.LaundromatProperties p input').removeAttr('disabled');
                    $('.LaundromatProperties').removeClass('grey lighten-3');
                    $('.HomeProperties').addClass('grey lighten-3');
                }
                else {
                    //The user wants their laundry done at the laundry mart
                    $('.LaundromatProperties p input').attr('disabled', "disabled");
                    $('.HomeProperties p input').removeAttr('disabled');
                    $('.LaundromatProperties').addClass('grey lighten-3');
                    $('.HomeProperties').removeClass('grey lighten-3');
                }
            }
        });
        //Dry clean Staff step 2
        $('#BtnDryClenStep1').click(function (e) {
            $.ajax({
                url: rootPath + 'SendX/GetClosestDryCleaners',
                success: function (answer) {
                    if (answer == 'false') {
                        $('#RealDryCleanMapLocations').hide('slow');
                    }
                    else {
                        var result = JSON.parse(answer);
                        console.log(result);
                        $('#FakeDryCleanMapLocations').show('slow');
                    }
                    $('#DryCleanMapLoading').hide('slow');
                }
            });
            AnimateDiv('#DryCleanItems', '#DryCleanMapLocations');
            $('#DryCleanMapLoading').show('slow');
        });
        //Go back to Dryclean Staff step 1 
        $('#BtnDryClenStep1Prv').click(function (e) {
            //Compare with upper method
        });
    }
});
//Deal with the SEEN functionallity
$(document).on("mouseenter", ".ThePost", function (e) {
    var postID = $(e.currentTarget).attr("PostID");
    var seen = $(e.currentTarget).attr("Seen");
    var link = $(e.currentTarget).attr("Link");
    var views = $(e.currentTarget).children().find('.PostViews');
    if (seen != "true") {
        $(e.currentTarget).attr("Seen", "true");
        $.ajax({
            url: link + "?PostID=" + postID + "&currentViews=" + views.html(),
            success: function (result) {
                views.html(result);
            }
        });
    }
});
//Used to enable and Disable classified
function TriggerClassified(e) {
    if ($(e.currentTarget).prop('checked')) {
        if ($('#WhatServiceL').prop('checked')) {
            //This means the classified wants laundry
            $('.LaundryProperties').hide('slide-right');
            $('#ClassifiedLaundry').show('slide-up');
            $('#ClassifiedDryClean').hide('slide-down');
        }
        else {
            //This means the classified wants dry clean
            $('.DrycleanProperties').hide('slide-right');
            $('#ClassifiedDryClean').show('slide-up');
            $('#ClassifiedLaundry').hide('slide-down');
        }
    }
    else {
        $('#ClassifiedDryClean').hide('slide-up');
        $('#ClassifiedLaundry').hide('slide-down');
        if ($('#WhatServiceL').prop('checked')) {
            $('.LaundryProperties').show('slide-left');
        }
        else {
            $('.DrycleanProperties').show('slide-left');
        }
    }
}
//Used to add chip to basket
function AddToBasket(e) {
    var price = $(e).find('span').attr('data-price');
    var weight = $(e).find('span').attr('data-weight');
    var type = $(e).find('span').html();
    $('#SendChips').append('<div class="chip ChipX"><input class="BasketChip" value="1" max="100" min="1" type="number" /><span class="text-white text-darken-4" data-price="' + price + '" data-weight="' + weight + '">' + type + '</span><i class="close material-icons">close</i></div>');
    $(e).remove();
    //AM HERE TODO // HANDLE ALL THE PRICE AND WEIGHT CHANGES HERE
    $('.ChipX input').on('change', function (e) {
        var quantity = +$(e.currentTarget).val();
        if (quantity >= 100) {
            $(e.currentTarget).val(99);
        }
    });
    //This is when the user clicks the small little close button
    $('.ChipX .close').on('click', function (e) {
        var price = $(e.currentTarget).parent().find('span').attr('data-price');
        var weight = $(e.currentTarget).parent().find('span').attr('data-weight');
        var type = $(e.currentTarget).parent().find('span').html();
        if (($('.ClothingTabs').html().indexOf(type) < 0)) {
            $('.ClothingTabs').prepend('<li><a class="chip SuggestionTag center text-white lighten-3"  onclick="AddToBasket(this)"><span class="text-white text-darken-4" data-price="' + price + '" data-weight="' + weight + '">' + type + '</span></a></li>');
        }
    });
}
//Call the search method and pass it the search to perform
function SearchFor(text) {
    var link = $(text).attr('link');
    var search = $(text).val();
    window.location.href = link + "?SearchText=" + search;
}
//The Actual Seach
function PerformTheSearch(searchhh, f, s, link) {
    $('#SearchPreloader').removeClass('hidden');
    var search = $(searchhh).val();
    var filter = $(f).val();
    var sort = $(s).val();
    if (search.length < 2) {
        var $ToastMessage = $('<span>Your search is too short</span>');
        Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
        $('#SearchPreloader').addClass('hidden');
        return;
    }
    $.ajax({
        url: link + "?Search=" + search + "&Filter=" + filter + "&Sort=" + sort,
        dataType: 'JSON',
        success: function (result) {
            if (result.length < 1) {
                $('#SearchResults').html('<div class="blog-sizer"></div><div class="card-panel">Sorry no results for ' + search + '</div>');
            }
            else {
                $('#SearchResults').html('<div class="blog-sizer"></div>' + result);
                //Initialize items
                $('html').imagesLoaded(function () {
                    $('.grid').masonry('reloadItems');
                    $('.grid').masonry('layout');
                });
                $('.materialboxed').materialbox();
                $('.modal-trigger').leanModal();
                $('.tooltipped').tooltip();
            }
            $('#SearchPreloader').addClass('hidden');
        }
    });
}
//Functionallity of the close button on the reveal of the post
function CloseRevealModal(RevealID) {
    $(RevealID).hide('slow');
}
//This method goes to c# and collect all the ratings of each admin and puts them to the stars
function GetAllStars(UserID) {
    $.ajax({
        url: rootPath + 'Admin/GetAllStars?UserID=' + UserID,
        success: function (result) {
            $(document).imagesLoaded(function () {
                $('.rateUser_' + UserID + "[value=" + result + "]").prop('checked', true);
            });
        }
    });
}
//Rate the user this will be used by the Laundromats method
function RateUser(userID, rateValue, raterID, link, target) {
    var UserID = +userID;
    var RaterID = +raterID;
    if (UserID == RaterID) {
        var $ToastMessage = $("<span>Sorry you can't rate yourself</span>");
        Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
        GetAllStars(UserID);
    }
    else {
        $.ajax({
            url: link + "?userID=" + UserID + "&RateValue=" + rateValue + "&RaterID=" + RaterID,
            success: function (result) {
                $(target).prop('checked', false);
                $(target + "[value=" + result + "]").prop('checked', true);
                GetAllStars(UserID);
                var $ToastMessage = $("<span>Thank you , your rate is counted</span>");
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
            }
        });
    }
}
//Ths function is used to go and collect the items after the user click on THE price
function GetAllClothes(postID) {
    $.ajax({
        url: rootPath + 'ReceiveX/GetAllClothes?PostID=' + postID,
        success: function (result) {
            var clothes = JSON.parse(result);
            $('#ClothingList_' + postID).html(clothes);
        }
    });
}
//This function is used when the user clicks on the model to view the comments ,,It goes to the database and take all the comments needed
function getAllComments(postID, numComments) {
    //Start the loading animation
    $('.ModalLoading').removeClass('hidden');
    $.ajax({
        url: rootPath + 'ReceiveX/GetAllComments?PostID=' + postID,
        success: function (result) {
            AnimateDiv('.ModalLoading', '#Comments_' + postID);
            $('.ModalLoading').addClass('hidden');
            if (result == "No comments") {
                var $ToastMessage = $('<span>Sorry we dont have comments here</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
            }
            else {
                var comments = JSON.parse(result);
                $('#Comments_' + postID).html(comments);
            }
            //Set up the events for wryting comments and stuff
            //This is used to perform an enter click when the user clicks enter
            $('.CommentOnPostInput').on('keyup', function (e) {
                if (e.which == 13) {
                    $(e.currentTarget).parent().find('.CommentOnPost').click();
                }
            });
            //This function is used by admins when the comment to a post
            $('.CommentOnPost').on('click', function (e) {
                var BtnComment = $(e.currentTarget);
                var TxtMessage = BtnComment.parent().find('.CommentOnPostInput').val();
                var link = BtnComment.attr('data-link');
                var userID = BtnComment.attr('data-userID');
                var postID = BtnComment.attr('data-postID');
                BtnComment.parent().find('input').val("");
                if (TxtMessage.length < 2) {
                    return;
                }
                $.ajax({
                    url: link + "?UID=" + userID + "&PID=" + postID + "&Message=" + TxtMessage,
                    success: function (result) {
                        if (result === "MaxComments") {
                            var $ToastMessage = $('<span>Sorry your comment was not sent , too many comments</span>');
                            Materialize.toast($ToastMessage, 3000, 'rounded lighten-3 ToastX z-depth-3');
                        }
                        else if (result === "Error") {
                            var $ToastMessage = $('<span>Sorry your comment was not sent , try again later</span>');
                            Materialize.toast($ToastMessage, 3000, 'rounded lighten-3 ToastX z-depth-3');
                        }
                        else {
                            //This will write the massage for the user to be fooled that it saved 'But it is saved dude' "The user must 1st refresh for them to see if (0_0)"
                            BtnComment.parent().parent().parent().parent().find('ul[comments]').append(result);
                            //Lets increment the number then
                            var count = +$('#nComments_' + postID).html();
                            count++;
                            $('#nComments_' + postID).html(count + "");
                        }
                    }
                });
            });
        }
    });
}
function AddWasherList(postID, washerID, washmessage) {
    DismissToasts();
    $.ajax({
        url: rootPath + 'ReceiveX/AddWasher?PostID=' + postID + '&washerID=' + washerID + '&washerMessage=' + washmessage,
        success: function (result) {
            if (result != "Done") {
                //This person is trying to help himself or already in the queue to help wash this
                var $ToastMessage = $('<span>' + result + '</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
            }
            else {
                var $ToastMessage = $('<span>Thank you,You are added to the list of people who want to wash this</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () {
                    var count = +$('#nHelp_' + postID).html();
                    count++;
                    $('#nHelp_' + postID).html(count + "");
                });
            }
        }
    });
}
//Bid to wash of a normal user will also redirect to AddwasherList
function ConfirmWannaWash(postID, washerID) {
    ConfirmToast("Are you sure ?", 'AddWasherList(\'' + postID + '\',\'' + washerID + '\',\'yes\')');
}
function DismissToasts() {
    $('#toast-container').html("");
}
function getAllHelpers(postID, numHelpers) {
    //Start the loading animation
    $('.ModalHelpersLoading').removeClass('hidden');
    $.ajax({
        url: rootPath + 'ReceiveX/GetAllHelpers?PostID=' + postID,
        success: function (result) {
            if (result == "No Helpers") {
                var $ToastMessage = $('<span>This post does not have helpers , be the first one to bid</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () {
                    $('.ModalHelpersLoading').addClass('hidden');
                });
            }
            else {
                $('.ModalHelpersLoading').addClass('hidden');
                AnimateDiv('.ModalLoading', '#Helpers_' + postID);
                var helpers = JSON.parse(result);
                $('#Helpers_' + postID).html(helpers);
            }
        }
    });
}
//This function is used to transite from one div to the other in an animated way
/**
 *
 * @param div1ID Div to be removed
 * @param div2ID Div to be added
 * @param isGoingBack Changes the animation if true
 */
function AnimateDiv(div1ID, div2ID, isGoingBack) {
    if (isGoingBack === void 0) { isGoingBack = false; }
    if (isGoingBack) {
        $(div1ID).addClass(animateShake).one(animateVenders, function (es) {
            $(div1ID).removeClass(animateShake);
            $(div1ID).addClass('hidden');
            $(div2ID).removeClass('hidden');
            $(div2ID).addClass('animated fadeInLeft').one(animateVenders, function (es) {
                $(div2ID).removeClass('animated fadeInLeft');
            });
        });
    }
    else {
        $(div1ID).addClass(animateToLeft).one(animateVenders, function (es) {
            $(div1ID).removeClass(animateToLeft);
            $(div1ID).addClass('hidden');
            $(div2ID).removeClass('hidden');
            $(div2ID).addClass(animateToRight).one(animateVenders, function (es) {
                $(div2ID).removeClass(animateToRight);
            });
        });
    }
}
var Validate = [false, false, false, false, false, true, false, false];
//This is used to send the user's Message to Us
function SubmitTalkToUs(link, ContactUsName, ContactUsEmail, ContactUsMessage, ContactUsErrors) {
    $(ContactUsErrors).html("");
    $(ContactUsName).removeClass('incorrect');
    $(ContactUsEmail).removeClass('incorrect');
    $(ContactUsMessage).removeClass('incorrect');
    var Name = $(ContactUsName).val();
    var Email = $(ContactUsEmail).val();
    var Message = $(ContactUsMessage).val();
    if (Name.length < 3 || Email.search("^\\S+@\\S+$") === -1 || Message.length < 10) {
        if (Message.length < 10) {
            $(ContactUsErrors).html("Your message must be longer than that");
            $(ContactUsMessage).addClass('incorrect');
        }
        if (Email.search("^\\S+@\\S+$") === -1) {
            $(ContactUsErrors).html("Enter a valid email please");
            $(ContactUsEmail).addClass('incorrect');
        }
        if (Name.length < 3) {
            $(ContactUsErrors).html("Name must be 4 or more charactors long");
            $(ContactUsName).addClass('incorrect');
        }
        return;
    }
    //Call ajax to send you message
    $.ajax({
        url: link + "?Name=" + Name + "&Email=" + Email + "&Message=" + Message,
        success: function (result) {
            if (result == "Done") {
                var $ToastMessage = $('<span>Thank you,Your message is sent</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { window.location.reload(); });
            }
            else {
                $(ContactUsErrors).html(result);
                return;
            }
        }
    });
}
//Variables to walk with this method (DONT SEPARATE THEM)
var SendItemNames = [];
var SendItemQuantity = [];
var properties = [];
//Used to submit the user's clothes
function SendClothesSubmit(link, M, TD, DD, P, userID, UserHasLocation) {
    $('#SendError').html("");
    $('#SendErrorResolve').html("");
    $(DD).removeClass('incorrect');
    $(TD).removeClass('incorrect');
    var Price = $(P).val();
    var Message = $(M).val();
    var TimeDue = $(TD).val();
    var DateDue = $(DD).val();
    if (Price.toString().length < 1) {
        $(P).addClass('incorrect');
        $('#SendError').html("Please enter a price");
        $('#SendClothesSubmit').addClass(animateShake).one(animateVenders, function (es) {
            $('#SendClothesSubmit').removeClass(animateShake);
        });
        return;
    }
    if (DateDue.length < 4) {
        $(DD).addClass('incorrect');
        $('#SendError').html("Please select a date");
        $('#SendClothesSubmit').addClass(animateShake).one(animateVenders, function (es) {
            $('#SendClothesSubmit').removeClass(animateShake);
        });
        return;
    }
    if (TimeDue.length < 3) {
        $(TD).addClass('incorrect');
        $('#SendError').html("Please select due time");
        $('#SendClothesSubmit').addClass(animateShake).one(animateVenders, function (es) {
            $('#SendClothesSubmit').removeClass(animateShake);
        });
        return;
    }
    if ($('#SendPrice').hasClass('incorrect')) {
        $('#SendClothesSubmit').addClass(animateShake).one(animateVenders, function (es) {
            $('#SendClothesSubmit').removeClass(animateShake);
        });
        return;
    }
    if (UserHasLocation == "true") {
        var $ToastMessage = $('<span>Sorry your message was not sent because we do not have your location</span>');
        Materialize.toast($ToastMessage, 5000, 'rounded lighten-3 ToastX z-depth-3', function () {
            var $ToastMessage = $('<span>Please log out and log in again to solve this problem</span>');
            Materialize.toast($ToastMessage, 7000, 'rounded lighten-3 ToastX z-depth-3');
        });
        $('#SendClothesSubmit').addClass(animateShake).one(animateVenders, function (es) {
            $('#SendClothesSubmit').removeClass(animateShake);
        });
        return;
    }
    var Type = 1;
    //If the laundry is been done at the laundromat change the type
    if ($('#WhereToWashL').prop('checked')) {
        Type = 0;
    }
    $.ajax({
        method: "POST",
        url: link + "?UserID=" + userID + "&Message=" + Message + "&Price=" + Price + "&DateDue=" + DateDue + "&TimeDue=" + TimeDue + "&Items=" + SendItemNames + "&Quantity=" + SendItemQuantity + "&Properties=" + properties + "&Type=" + Type,
        success: function (result) {
            if (result == "Done") {
                var $ToastMessage = $('<span>Thank you,Your message is uploaded someone will help you in no time</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { window.location.reload(); });
            }
            else {
                $('#SendError').html(result);
                return;
            }
        }, error: function (er) {
            console.log("Error " + er);
        }
    });
}
/**
 * This functioon is used to validate the editted profile that the user is trying to submit
 * @param CardType whether personal , contact or location
 */
function EditSaveChanges(CardType, UserID) {
    if (CardType == 'personal') {
        $('#EditPersonalErrors').html('').removeClass("red-text").removeClass("teal-text");
        var Name = $('#EditName').val();
        if (Name.length < 2) {
            $('#EditPersonalPreloader').addClass("hidden");
            $('#EditPersonalErrors').html("A name is required with more than 2 charactors").addClass("red-text");
            return;
        }
        var Surname = $('#EditSurname').val();
        if (Surname.length < 2) {
            $('#EditPersonalPreloader').addClass("hidden");
            $('#EditPersonalErrors').html("A surname is required with more than 2 charactors").addClass("red-text");
            return;
        }
        var Email = $('#EditEmail').val();
        var Age = +$('#EditAge').val();
        if (isNaN(Age)) {
            $('#EditPersonalPreloader').addClass("hidden");
            $('#EditPersonalErrors').html("Age is required").addClass("red-text");
            return;
        }
        var Desc = $('#EditDesc').val();
        if (Desc.length < 4) {
            $('#EditPersonalPreloader').addClass("hidden");
            $('#EditPersonalErrors').html("A Description is required with more than 4 charactors").addClass("red-text");
            return;
        }
        $.ajax({
            type: 'POST',
            url: rootPath + "Account/EditPersonal?UserID=" + UserID + "&Name=" + Name + "&Surname=" + Surname + "&Email=" + Email + "&Age=" + Age + "&Desc=" + Desc,
            success: function (result) {
                $('#EditPersonalPreloader').addClass("hidden");
                $("#EditPersonalErrors").html("Changes saved").addClass("teal-text");
                return;
            }
        });
    }
    else if (CardType == 'homelocation') {
        $.ajax({
            method: "POST",
            url: rootPath + "Account/SetLocation/?lat=" + Edit_Home_Location['lat'] + "&lon=" + Edit_Home_Location['lon'] + "&Country=" + Edit_Home_Location['Country'] + "&Province=" + Edit_Home_Location['Province'] + "&City_TownName=" + Edit_Home_Location['City_TownName'] + "&LocalName=" + Edit_Home_Location['LocalName'] + "&StreetName=" + Edit_Home_Location['StreetName'] + "&HouseNumber=" + Edit_Home_Location['HouseNumber'] + "&Which=home",
            success: function (result) {
                $.ajax({
                    url: rootPath + 'SendX/GetUserLocations?UserID=' + UserID,
                    success: function (answer) {
                        var result = JSON.parse(answer);
                        if (result[0] != "No Location") {
                            result[0] = JSON.parse(result[0]);
                            $('#Edit_HomeLocationText_Old').html(ConvertCollectAt(result[0]));
                        }
                        else {
                            $('#Edit_HomeLocationText_Old').html(ConvertCollectAt(null));
                        }
                        if (result[1] != "No Location") {
                            result[1] = JSON.parse(result[1]);
                            $('#Edit_WorkLocationText_Old').html(ConvertCollectAt(result[1]));
                        }
                        else {
                            $('#Edit_WorkLocationText_Old').html(ConvertCollectAt(null));
                        }
                    }
                });
            }
        });
    }
    else if (CardType == 'worklocation') {
        $.ajax({
            method: "POST",
            url: rootPath + "Account/SetLocation/?lat=" + Edit_Work_Location['lat'] + "&lon=" + Edit_Work_Location['lon'] + "&Country=" + Edit_Work_Location['Country'] + "&Province=" + Edit_Work_Location['Province'] + "&City_TownName=" + Edit_Work_Location['City_TownName'] + "&LocalName=" + Edit_Work_Location['LocalName'] + "&StreetName=" + Edit_Work_Location['StreetName'] + "&HouseNumber=" + Edit_Work_Location['HouseNumber'] + "&Which=work",
            success: function (result) {
                $.ajax({
                    url: rootPath + 'SendX/GetUserLocations?UserID=' + UserID,
                    success: function (answer) {
                        var result = JSON.parse(answer);
                        if (result[0] != "No Location") {
                            result[0] = JSON.parse(result[0]);
                            $('#Edit_HomeLocationText_Old').html(ConvertCollectAt(result[0]));
                        }
                        else {
                            $('#Edit_HomeLocationText_Old').html(ConvertCollectAt(null));
                        }
                        if (result[1] != "No Location") {
                            result[1] = JSON.parse(result[1]);
                            $('#Edit_WorkLocationText_Old').html(ConvertCollectAt(result[1]));
                        }
                        else {
                            $('#Edit_WorkLocationText_Old').html(ConvertCollectAt(null));
                        }
                    }
                });
            }
        });
    }
    else if (CardType == 'addcompany') {
        //The user is an admin and Has a company dude
        var CompanyName = $('#EditCampanyName').val();
        if (CompanyName.length < 4) {
            $('#EditProfilePreloader').addClass("hidden");
            $('#EditAddCompanyErrors').html("A company name is required with more than 4 charactors");
            return;
        }
        var CompanyAddress = $('#EditCampanyAddress').val();
        if (CompanyAddress.length < 4) {
            $('#EditProfilePreloader').addClass("hidden");
            $('#EditAddCompanyErrors').html("A company address is required with more than 4 charactors");
            return;
        }
        var CompanyWapsite = $('#EditCampanyWapsite').val();
        var CompanyAbout = $('#EditCampanyAbout').val();
        if (CompanyAbout.length < 20) {
            $('#EditProfilePreloader').addClass("hidden");
            $('#EditAddCompanyErrors').html("You must write info about " + CompanyName + " with no less than 20 charactors");
            return;
        }
        var CompanyTell = +$('#EditCampanyTell').val();
        //This is for users with companies
        $.ajax({
            type: 'POST',
            url: rootPath + "Account/EditAddCompany?UserID=" + UserID + "&CompanyName=" + CompanyName + "&CompanyAddress=" + CompanyAddress + "&CompanyWapsite=" + CompanyWapsite + "&CompanyAbout=" + CompanyAbout + "&CompanyTell=" + CompanyTell,
            success: function (result) {
                $('#EditProfilePreloader').addClass("hidden");
                if (result == "Done") {
                    //Refresh the page "Coz the user is Logged in" and show a toastmessage
                    var $ToastMessage = $('<span>Congratulations ' + Name + ' your profile is updated , enjoy!</span>');
                    Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
                    return;
                }
                $("#EditAddCompanyErrors").html(result);
                return;
            }
        });
    }
}
//This function is used to hide and show company info on edit profile page
function HideOrShowCompanyInfo() {
    var Checked = $('#EditRadioCompany').attr('Xcheck');
    if (Checked === "checked") {
        $('#CompanyProperties').addClass('hidden');
        $('#EditRadioCompany').attr('Xcheck', 'unchecked');
    }
    else {
        $('#CompanyProperties').removeClass('hidden');
        $('#EditRadioCompany').attr('Xcheck', 'checked');
    }
}
//This funtion is used tp select a gender for a user
function RegChangeGender() {
    var Checked = $('#RegGenderSet').attr('Xcheck');
    if (Checked === "Male") {
        $('#RegGenderSet').attr('Xcheck', 'Female');
    }
    else {
        $('#RegGenderSet').attr('Xcheck', 'Male');
    }
}
//This function is used to hide and show the email option in the registration form
function HideOrShowEmail() {
    var Checked = $('#RegRadioEmail').attr('Xcheck');
    if (Checked === "checked") {
        $('#RegEmailActivation').addClass('hidden');
        $('#RegRadioEmail').attr('Xcheck', 'unchecked');
        Validate[6] = false;
    }
    else {
        $('#RegEmailActivation').removeClass('hidden');
        $('#RegRadioEmail').attr('Xcheck', 'checked');
        Validate[6] = true;
    }
}
//Register The user
function RegisterUser(link, HomeLink) {
    //Start the Preloader
    $('#RegisterPreloader').removeClass("hidden");
    //Reset the error validator
    $("#RegistrationErrors").html("");
    var HasEmail = true;
    var validationPassed = true;
    Validate.forEach(function (value, i) {
        if (value === false || i === 4) {
            if (i === 4) {
                if (Validate[6] === true && value === false) {
                    //The user told us that he has an email but doesnot give it to us
                    validationPassed = false;
                }
                else if (Validate[6] === false) {
                    HasEmail = false;
                }
            }
            else if (i === 6) {
                //We do not have to validate this 'Coz it is for If the user has an email or not'
            }
            else {
                //This means the specific value is FALSE and we dnt want that
                validationPassed = false;
            }
        }
    });
    if (!(validationPassed)) {
        $('#RegisterPreloader').addClass("hidden");
        var $ToastMessage = $('<span>You have some errors up there , Go and fix them first</span>');
        Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3');
        return;
    }
    //This variables are already fine DUDE dont stress
    var gender = $('#RegGenderSet').attr('Xcheck');
    var email = $('#RegEmail').val();
    if (!(HasEmail)) {
        email = "noemail";
    }
    var password = $('#RegPassword').val();
    var contact = $('#RegContact').val();
    var surname = $('#RegSurname').val();
    var name = $('#RegName').val();
    //Check if the contact exists
    $.ajax({
        type: 'GET',
        url: link + "?contact=" + contact + "&name=" + name + "&surname=" + surname + "&password=" + password + "&email=" + email + "&gender=" + gender,
        success: function (result) {
            $('#RegisterPreloader').addClass("hidden");
            if (result == "Done") {
                var $ToastMessage = $('<span>Congratulations ' + (gender == "Male" ? 'Mr ' : 'Mrs ') + surname + ' you are now a member of LaundromatX , enjoy!</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { window.location.href = HomeLink; });
                return;
            }
            $("#RegistrationErrors").html(result);
            return;
        }
    });
}
//Used to toggle the prefab of log in
function ToggleLogInFAB() {
    $('.fixed-RedirectAction-btn').closeFAB();
}
//This is used to remove a company by the admin on the edit profile page
function RemoveAComment(link, companyId, adminId, userId) {
    var adminID = +adminId;
    var companyID = +companyId;
    var userID = +userId;
    if (isNaN(adminID) || isNaN(companyID) || isNaN(userID)) {
        var $ToastMessage = $('<span>Sorry unable to delete that company</span>');
        Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
        return;
    }
    $.ajax({
        method: 'POST',
        url: link + "?AdminId=" + adminID + "&CompanyID=" + companyID + "&UserID=" + userID,
        success: function (result) {
            if (result == "Done") {
                $("#CompanyRow_" + companyID).hide('slow');
            }
            else {
                var $ToastMessage = $('<span>' + result + '</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
            }
        },
        error: function (result) {
            var $ToastMessage = $('<span>Sorry unable to delete that company</span>');
            Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () { });
        }
    });
}
//Used on the onclick of submitting a login from the modal
function LogInUser(Contact, Pass, link) {
    $('#LogInErrors').html("");
    $('#LogInPreloader').removeClass("hidden");
    $('.LogInSubmit').hide();
    $('#LogInModal').removeClass("ErrorModal");
    var number = +$(Contact).val();
    var password = $(Pass).val();
    if (number === NaN) {
        $('#LogInErrors').html("Please fill in the correct contact numbers");
        $('#LogInPreloader').addClass("hidden");
        $('.LogInSubmit').show();
        $('#LogInModal').addClass("ErrorModal");
        $('.LogInSubmit').addClass(animateShake).one(animateVenders, function (es) {
            $('.LogInSubmit').removeClass(animateShake);
        });
        return;
    }
    if (number.toString().length != 9) {
        $('#LogInErrors').html("Please fill in the correct contact numbers");
        $('#LogInPreloader').addClass("hidden");
        $('.LogInSubmit').show();
        $('#LogInModal').addClass("ErrorModal");
        $('.LogInSubmit').addClass(animateShake).one(animateVenders, function (es) {
            $('.LogInSubmit').removeClass(animateShake);
        });
        return;
    }
    if (password.length < 4) {
        $('#LogInErrors').html("Please fill in the correct password");
        $('#LogInPreloader').addClass("hidden");
        $('.LogInSubmit').show();
        $('#LogInModal').addClass("ErrorModal");
        $('.LogInSubmit').addClass(animateShake).one(animateVenders, function (es) {
            $('.LogInSubmit').removeClass(animateShake);
        });
        return;
    }
    $.ajax({
        type: 'POST',
        url: link + '?num=' + number + '&pass=' + password,
        success: function (result) {
            if (result !== "Done") {
                $('#LogInErrors').html(result);
                $('#LogInPreloader').addClass("hidden");
                $('.LogInSubmit').show();
                $('#LogInModal').removeClass("ErrorModal");
                $('.LogInSubmit').addClass(animateShake).one(animateVenders, function (es) {
                    $('.LogInSubmit').removeClass(animateShake);
                });
            }
            else {
                $('#LogInPreloader').addClass("hidden");
                $('.LogInSubmit').show();
                $('#LogInModal').addClass("ErrorModal");
                $('#LogInModal').closeModal();
                var $ToastMessage = $('<span>Coolies you are now logged in</span>');
                Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3', function () {
                    localStorage.setItem("IsloggedIn", "true");
                    localStorage.setItem("IsChecked", "false");
                    window.location.reload();
                });
            }
        }
    });
}
//DRY CLEAN STUFF
//When the user increase the number of dry clean items
$('.DryCleanNumber').on('change', function (e) {
    $('.DryCleanItem').click();
});
//Toggle dry clean iteam
function ToggleDryCleanItem(item) {
    var isAdded = $(item).attr('data-Added');
    var Nitems = Number($(item).find('.DryCleanNumber').val());
    if (isNaN(Nitems) || Nitems < 1) {
        isAdded = "false";
    }
    if (isNaN(Nitems) || Nitems < 1 && isAdded == 'false') {
        $(item).find('.DryCleanMoreInfo').hide('slow');
        $(item).attr('data-Added', "true");
        $(item).find('.DryCleanNumber').val(0);
        $(item).addClass('grey');
        $(item).find('.DryCleanItemPrice').html("");
    }
    else {
        var price = Number(Number($(item).attr('data-Price')) * Number($(item).find('.DryCleanNumber').val()));
        $(item).find('.DryCleanMoreInfo').show('slow');
        $(item).attr('data-Added', "false");
        $(item).removeClass('grey');
        $(item).find('.DryCleanItemPrice').html(price + "");
        $('#DryCleanAddedItems').html($(item).html());
        $(item).html('Blank');
    }
}
//This is a helper function just to bring up a toast of yes or no!!!
function ConfirmToast(message, Yes, No) {
    if (message === void 0) { message = "Are you sure?"; }
    if (No === void 0) { No = "DismissToasts()"; }
    var $ToastMessage = $('<div><div><p class="center text-center">' + message + '</p></div>' +
        '<div class="row">' +
        '<div class="col s6"><a style="width:100%" role="button" class="btn waves-effect teal" onclick="' + Yes + '">Yes</a></div>' +
        '<div class="col s6"><a style="width:100%" role="button" class="btn waves-effect red" onclick="' + No + '">No</a></div>' +
        '</div></div>');
    Materialize.toast($ToastMessage, 9990000, 'rounded darken-1 red white-text', function () { });
}
//This function is when a helper decides to stop helping in a post
function RemoveHelper(helperID) {
    DismissToasts();
    $.ajax({
        method: "POST",
        url: rootPath + 'ReceiveX/RemoveHelper?helperID=' + helperID,
        success: function (result) {
            if (result == "done") {
                var POSTID = $("#PostHelperLI_" + helperID + "").attr("data-postID");
                $("#PostHelperLI_" + helperID + "").hide('slow');
                var count = +$('#nHelp_' + POSTID).html();
                count--;
                if (count >= 0) {
                    $('#nHelp_' + POSTID).html(count + "");
                }
            }
            else {
                alert(result);
            }
        }
    });
}
//This function is when a post owner wanna accept a helper to do their laundry
function AcceptHelper(helperID) {
    DismissToasts();
    $.ajax({
        method: "POST",
        url: rootPath + 'ReceiveX/AcceptHelper?helperID=' + helperID,
        success: function (result) {
            console.log(result);
        }
    });
}
//This is used to go the the database and get all the notifications for a specific user
function GetAllNotifications(UserID) {
    if (isNaN(+UserID)) {
        return;
    }
    $.ajax({
        method: "POST",
        url: rootPath + 'Notify/GetNotifications?UserID=' + UserID,
        success: function (result) {
            var notifications = JSON.parse(result);
            $('#notification_List').html(notifications);
        }
    });
}
//When the user has seen a notification
function PerformNotifcationSeen(NotifyID, self) {
    var UserID = +$("head").attr("data-UserID");
    if (!isNaN(UserID)) {
        if (isNaN(+NotifyID)) {
            return;
        }
        $.ajax({
            method: "POST",
            url: rootPath + 'Notify/NotificationSeen?NotifyID=' + NotifyID,
            success: function (result) {
                $(".NotificationLi#NotificationLi_" + NotifyID).hide('slow');
                CheckNotifications();
                var ID = +$(self).attr('data-ID');
                var ref = $(self).attr('data-RefName');
                if (!isNaN(ID)) {
                    window.location.href = rootPath + "Notify/b?ID=" + ID + "&UserID=" + UserID + "&Type=" + ref;
                }
            }
        });
    }
    else {
        var $ToastMessage = $('<span>You must be logged in</span>');
        Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3');
    }
}
//This will be called in an interval to check for notifications
function CheckNotifications() {
    //Universal UserID
    var UserID = +$("head").attr("data-UserID");
    if (!isNaN(UserID)) {
        $.ajax({
            method: "POST",
            url: rootPath + 'Notify/NotificationsCount?UserID=' + UserID,
            success: function (total) {
                var count = +total;
                if (($("body[home]").html() != undefined)) {
                    if (count <= 0) {
                        $('.NotificationShaker').removeClass("animated flash infinite");
                    }
                    else {
                        $('.NotificationShaker').addClass("animated flash infinite");
                    }
                }
                else {
                    if (count <= 0) {
                        //We do not have notifications
                        $("#notifications-count").hide("slow");
                    }
                    else {
                        $("#notifications-count").show("slow");
                        $("#notifications-count").html(count + "");
                    }
                }
            }
        });
    }
}
//This is used on the account index to go and fetch more posts on button click
function LoadMorePosts(userID) {
    var index = $('.ThePost').toArray().length;
    $('#LoadingPosts').removeClass('hidden');
    $.ajax({
        url: rootPath + 'Account/LoadMorePosts?userID=' + userID + '&index=' + index,
        success: function (answer) {
            var i = 0;
            var result = JSON.parse(answer);
            if (result == "No more Posts") {
                if ($('#toast-container').children().length < 1) {
                    var $ToastMessage = $('<span>No more posts left</span>');
                    Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3');
                }
            }
            else {
                for (i = 0; i < result.length; i++) {
                    $('#AccountPosts').append(result[i]);
                }
                //Start the masonry
                $('.grid').imagesLoaded(function () {
                    $('.grid').masonry('reloadItems');
                    $('.grid').masonry('layout');
                });
                //Initialize items
                $('.materialboxed').materialbox();
                $('.modal-trigger').leanModal();
                $('.tooltipped').tooltip();
            }
            $('#LoadingPosts').addClass('hidden');
        }
    });
}
//This is used on the account index to go and fetch more pending posts on button click
function LoadMorePendingPosts(userID) {
    var index = $('.ThePost').toArray().length;
    $('#LoadingPendingPosts').removeClass('hidden');
    $.ajax({
        url: rootPath + 'Account/LoadMorePendingPosts?userID=' + userID + '&index=' + index,
        success: function (answer) {
            var i = 0;
            var result = JSON.parse(answer);
            if (result == "No more Posts") {
                if ($('#toast-container').children().length < 1) {
                    var $ToastMessage = $('<span>No more posts left</span>');
                    Materialize.toast($ToastMessage, 4000, 'rounded lighten-3 ToastX z-depth-3');
                }
            }
            else {
                for (i = 0; i < result.length; i++) {
                    $('#PendingPosts').append(result[i]);
                }
                //Start the masonry
                $('.grid').imagesLoaded(function () {
                    $('.grid').masonry('reloadItems');
                    $('.grid').masonry('layout');
                });
                //Initialize items
                $('.materialboxed').materialbox();
                $('.modal-trigger').leanModal();
                $('.tooltipped').tooltip();
            }
            $('#LoadingPendingPosts').addClass('hidden');
        }
    });
}
//This is used on the chat to send a new message
function Chat_SendMessage(that, HelpID, RecieverID, SenderID) {
    var msg = $(that).parent().find('.ChatTextBox').val();
    $(that).parent().find('.ChatTextBox').val(' ');
    if (msg.length < 2) {
        var $ToastMessage = $('<span>Message too short!</span>');
        Materialize.toast($ToastMessage, 3000, 'rounded lighten-3 ToastX z-depth-3');
    }
    else {
        $.ajax({
            url: rootPath + 'Notify/SendMessage?HelpID=' + HelpID + '&SenderID=' + SenderID + '&ReceiverID=' + RecieverID + '&msg=' + msg,
            success: function (result) {
                $('.OrderChat').append("\n                          <div class='card-panel RightChat col s7 right grey lighten-4'>\n                          <p>\n                                " + msg + "\n                          </p>\n                          <span class='small blue-text'>now</span>\n                </div>\n                ");
            }
        });
    }
}
function LoadOrder(that, helpID) {
    if (!isNaN(+helpID)) {
        $.ajax({
            url: rootPath + 'notify/LoadOrder?helpID=' + helpID,
            success: function (result) {
                $('.BtnLoadOrder').removeClass('z-depth-3');
                $(that).addClass('z-depth-3');
                console.log(result);
                $('#OrderPanel').html(result);
            }
        });
    }
}
//This is used as a helper method to format a location
function ConvertCollectAt(location) {
    if (location == null) {
        return "\n                                    <p>\n                                    HouseNumber: None\n                                    </p>\n                                    <p>\n                                    StreetName: None\n                                    </p>\n                                    <p>\n                                    LocalName: None\n                                    </p>\n                                    <p>\n                                    City_TownName: None\n                                    </p>\n                                    <p>\n                                    Province: None\n                                    </p>\n                                    <p>\n                                    Country: None\n                                    </p>\n                                    ";
    }
    else {
        return "\n                                    <p>\n                                    HouseNumber: " + location['HouseNumber'] + "\n                                    </p>\n                                    <p>\n                                    StreetName: " + location['StreetName'] + "\n                                    </p>\n                                    <p>\n                                    LocalName: " + location['LocalName'] + "\n                                    </p>\n                                    <p>\n                                    City_TownName: " + location['City_TownName'] + "\n                                    </p>\n                                    <p>\n                                    Province: " + location['Province'] + "\n                                    </p>\n                                    <p>\n                                    Country: " + location['Country'] + "\n                                    </p>\n                                    ";
    }
}
//# sourceMappingURL=LaundramatX.js.map