$(document).ready(function () {

    /**** MyCart/Index ****/
    /*expand*/
    $("#mycart-index .expand").click(function () {
        var index = $(this).parent().parent().attr("data-index");
        var options = $(".options[data-index='" + index + "']");
        options.slideToggle();
    })

    $("#mycart-index .option").click(function () {
        var otherops = $(this).parent().find(".option.picked");
        otherops.removeClass("picked");
        otherops.find(".button").addClass("hide");

        $(this).addClass("picked");
        $(this).find(".button").removeClass("hide");
    })

    /**** Users/Details ****/
    /*switch tab*/
    $("#users-details .navigation li").click(function(){
        $(this).parent().find("li").removeClass("picked");
        $(this).addClass("picked");
        var index = $(this).data("index");
        $(".tab").addClass("hide");
        $(".tab[data-index='" + index + "']").removeClass("hide");

    });

    /**** MyAddresses/Edit, MyAddresses/Create ****/
    /*load subcategories*/
    var country = $("#SelectedCountryId");
    var city = $("#SelectedCityId");
    var citylabel = city.parent().find(".citylabel");

    //when validation occur
    if ($("#SelectedCityId option:selected").val() == "") {
        country.find("option").removeAttr("selected");
    }

    country.change(countryAjax);
    function countryAjax() {
        var id = { "id": $(this).val() }

        $.ajax({
            url: "/MyAddresses/GetCities",
            data: JSON.stringify(id),
            type: "POST",
            cache: false,
            contentType: 'application/json; charset=utf-8',
        })
            .done(function (response) {               

                if (response.length == 0) {
                    city.removeClass("display").addClass("hide");
                    citylabel.removeClass("display").addClass("hide");
                    city.find("option").removeAttr("selected");
                }
                else {
                    city.empty();
                    $('<option />', { value: "", text: "Select City" }).appendTo(city);
                    $.each(response, function (Index) {
                        $('<option />', { value: response[Index].Id, text: response[Index].Name }).appendTo(city);
                    });

                    if (city.hasClass("hide") && citylabel.hasClass("hide")) {
                        city.removeClass("hide").addClass("display");
                        citylabel.removeClass("hide").addClass("display");
                    }
                }

            })
            .fail(function () {
                alert("Error");
            })
    }

    /**** MyProducts/Details, Products/Details *****/
    /*gallery*/
    $(".gallery .navigation a.next").click(function () {

        var current = $(".gallery .container li.selected");
        var next = current.next("li");

        if (next.length > 0) {
            current.removeClass("selected");
            next.addClass("selected");
        }

    });
    $(".gallery .navigation a.previous").click(function () {

        var current = $(".gallery .container li.selected");
        var previous = current.prev("li");

        if (previous.length > 0) {
            current.removeClass("selected");
            previous.addClass("selected");
        }
    });

    /**** MyProducts/Create, MyProducts/Edit ****/
    /*add filechooser*/
    $(".gallery .add").click(function () {
        var filechooser = $('<div class="filechooser" />');
        var input = $('<input class="input file" type="file" name="files">');

        var prevId = $(this).parent().find(".filechooser:last-of-type .input.file").attr("id");
        if (prevId != null) {
            var Id = parseInt(prevId[prevId.length - 1]) + 1;
            var newId = "file" + Id;
            input.appendTo(filechooser);
            input.attr("id", newId);
            filechooser.insertAfter($(this).parent().find(".filechooser:last-of-type"));
        }
        else
        {
            input.appendTo(filechooser);
            input.attr("id", "file1");
            filechooser.insertAfter($(this).parent().find(".images"));
        }
        
    });

    /**** MyProducts/Create, MyProducts/Edit ****/
    /*load subcategories*/
    var cat = $("#SelectedCategoryId");
    var subcat = $("#SelectedSubCategoryId");    

    //when validation occur
    $("#myproducts-create #SelectedCategoryId option").removeAttr("selected");
    if ($("#myproducts-edit #SelectedCategoryId option:selected").val() == "")
    {
        subcat.removeClass("display").addClass("hide");
    }
    if ($("#myproducts-edit #SelectedSubCategoryId option:selected").val() == "")
    {
        cat.find("option").removeAttr("selected");
        subcat.find("option").removeAttr("selected");
        subcat.removeClass("display").addClass("hide");
    }
    //on change
    cat.change(catAjax);
    function catAjax() {
        var id = { "id": $(this).val() }

        $.ajax({
            url: "/MyProducts/GetSubCategories",
            data: JSON.stringify(id),
            type: "POST",
            cache: false,
            contentType: 'application/json; charset=utf-8',
        })
            .done(function (response) {
                var subcatselect = $("#SelectedSubCategoryId");
                var subcatlabel = subcatselect.parent().find(".subcatlabel");

                if (response.length == 0) {
                    subcatselect.removeClass("display").addClass("hide");
                    subcatlabel.removeClass("display").addClass("hide");
                    subcatselect.find("option").removeAttr("selected");
                }
                else {
                    subcatselect.empty();
                    $('<option />', { value: "", text: "Select SubCategory" }).appendTo(subcatselect);
                    $.each(response, function (Index) {
                        $('<option />', { value: response[Index].Id, text: response[Index].Name }).appendTo(subcatselect);
                    });

                    if (subcatselect.hasClass("hide") && subcatlabel.hasClass("hide")) {
                        subcatselect.removeClass("hide").addClass("display");
                        subcatlabel.removeClass("hide").addClass("display");
                    }
                }

            })
            .fail(function () {
                alert("Error");
            })
    }

    /**** MyCart ****/
    /*Checkout*/
    $("#mycart-index .option .button").click(function () {
        var button = $(this)
        var array = [];
        var buyrequests = button.parent().find(".cycle .buyrequest");

        buyrequests.each(function () {
            var giveId = parseInt($(this).find(".giveId").html());
            var getId = parseInt($(this).find(".getId").html());

            array.push({ "GiveBuyRequestId": giveId, "GetBuyRequestId": getId });
        });

        $.ajax({
            url: "/MyTransactions/Checkout",
            data: JSON.stringify(array),
            type: "POST",
            cache: false,
            contentType: 'application/json; charset=utf-8',
        })
            .done(function (html) {
                window.location.href = "/MyTransactions/Index";
            })
            .fail(function () {
                console.log("Error");
            })
    });

    /**** MyMatches ****/
    /*expand rows*/
    var mainRow = $("#mymatches .match .row.main");
    var optionRow = $("#mymatches .match .row.option");

    mainRow.click(function () {
        var options = $(this).parent().children(".row.option");
        options.slideToggle();
    })

    /**** MyTransactions/Details ****/
    /*rate buttons*/
    $("#mytransactions-details .status .myactionrate").not(".cursor-x").click(function () {
        var myaction = $(".myaction.rate");
        myaction.slideToggle();
    });

    /**** MyTransactions/Details ****/
    /*rate transaction*/
    $("#mytransactions-details .myaction.rate .button").click(function () {
        var button = $(this);
        var data = { "transactionbrId": $(".subtitle .tbrId").html(), "scorebr": button.attr("data-score") };
        $.ajax({
            url: "/MyTransactionBuyRequests/Rate",
            data: JSON.stringify(data),
            type: "POST",
            cache: false,
            contentType: 'application/json; charset=utf-8',
        })
            .done(function (html) {
                window.location.reload();
            })
            .fail(function () {
                button.parent().find(".error").html("Error");
            })
    });

    /**** MyTransactions/Details ****/
    /*Steps on hover*/
    $(".actions .names a").hover(function () {
        var index = $(this).index();
        var circle = $(this).parent().parent().find(".steps").children(".circle").eq(index);
        circle.attr('style', 'opacity: 0.8');
    }, function () {
        var index = $(this).index();
        var circle = $(this).parent().parent().find(".steps").children(".circle").eq(index);
        circle.attr('style', 'opacity: 1');
    });

    /**** MyTransactions/Details ****/
    /*Steps status show hide*/
    $(".actions .step").click(function () {
        var disabled = $(this).hasClass("disabled");
        var one = $(this).hasClass("one");
        var two = $(this).hasClass("two");
        var three = $(this).hasClass("three");

        if (one == true && disabled == false)
        {
            $(".message").not(".hide").addClass("hide");
            $(".myaction").hide();
            if ($(".status.two").hasClass("hide") == false)
            {
                $(".status.two").addClass("hide");
                $(".names .two").removeClass("bold");
            }
            if ($(".status.three").hasClass("hide") == false) {
                $(".status.three").addClass("hide");
                $(".names .three").removeClass("bold");
            }
            if ($(".status.one").hasClass("hide") == true)
            {
                $(".status.one").fadeIn("slow").removeClass("hide");
                $(".names .one").addClass("bold");
            }
        }
        else if (two == true && disabled == false)
        {
            $(".message").not(".hide").addClass("hide");
            $(".myaction").hide();
            if ($(".status.one").hasClass("hide") == false)
            {
                $(".status.one").addClass("hide");
                $(".names .one").removeClass("bold");
            }
            if ($(".status.three").hasClass("hide") == false) {
                $(".status.three").addClass("hide");
                $(".names .three").removeClass("bold");
            }
            if ($(".status.two").hasClass("hide") == true)
            {
                $(".status.two").fadeIn("slow").removeClass("hide");
                $(".names .two").addClass("bold");
            }
        }
        else if (three == true && disabled == false) {
            $(".message").not(".hide").addClass("hide");
            $(".myaction").hide();
            if ($(".status.one").hasClass("hide") == false) {
                $(".status.one").addClass("hide");
                $(".names .one").removeClass("bold");
            }
            if ($(".status.two").hasClass("hide") == false) {
                $(".status.two").addClass("hide");
                $(".names .two").removeClass("bold");
            }
            if ($(".status.three").hasClass("hide") == true) {
                $(".status.three").fadeIn("slow").removeClass("hide");
                $(".names .three").addClass("bold");
            }
        }
        else if (one == true && disabled == true) {
            if ($(".message.two").hasClass("hide") == false) $(".message.two").addClass("hide");
            if ($(".message.three").hasClass("hide") == false) $(".message.three").addClass("hide");
            if ($(".message.one").hasClass("hide") == true) $(".message.one").removeClass("hide");
        }
        else if (two == true && disabled == true)
        {
            if ($(".message.one").hasClass("hide") == false) $(".message.one").addClass("hide");
            if ($(".message.three").hasClass("hide") == false) $(".message.three").addClass("hide");
            if ($(".message.two").hasClass("hide") == true) $(".message.two").removeClass("hide");
        }
        else if (three == true && disabled == true)
        {
            if ($(".message.one").hasClass("hide") == false) $(".message.one").addClass("hide");
            if ($(".message.two").hasClass("hide") == false) $(".message.two").addClass("hide");
            if ($(".message.three").hasClass("hide") == true) $(".message.three").removeClass("hide");
        }

    });

    /**** Menu ****/
    var menu = $(".master .menu");
    $(".master .header .navicon a").click(function () {
        menu.toggleClass("mobile-show");
    })    
});