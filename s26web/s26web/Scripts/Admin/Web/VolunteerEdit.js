function Exchanged_record(k) {
    $(".dialog_table").hide();
    $(".dt_" + k).show();
    $("#dialog_exc").dialog({
        title: '您的兌換記錄',
        bgiframe: true,
        width: $(window).width() * 0.3,
        height: $(window).height(),
        modal: true,
        draggable: true,
        resizable: false,
        overlay: { opacity: 0.7, background: "#FF8899" },
        close: function () {
        },
        buttons: {
            '關閉': function () {
                $(this).dialog('close');
            }
        }
    });
}

$(function () {
    $("#dialog_exc").hide();
    $("#Go_Exchanged").click(function () {
        document.location.href = "";
    });
    $("#CityId").change(function () {
        $.ajax({
            type: "post",
            traditional: true,
            async: true,
            url: RootPath + "/shb/S26/Ajax_Area",
            data: { city: $(this).val(), zip: false, all: false },
            //dataType: "json",
            //jsonp: "callback",
            //jsonpCallback: "dool",
            success: function (data) {
                if (data) {
                    $("#AreaId2").html(data);
                }
            }
        });
    });
    $("#dialog_store").hide();
    $("#dialog_product").hide();
    var id = $("#Id").val();

    $(".invoice_2").first().change(function () {
        if ($(this).prop("checked")) {
            $("#invoice_changes").val("1");
        }
    });
    $("#Change_store_btn").click(function () {
        $("#dialog_store").dialog({
            title: '變更宅配店家',
            bgiframe: true,
            width: $(window).width() * 0.3,
            height: $(window).height() * 0.4,
            modal: true,
            draggable: true,
            resizable: false,
            overlay: { opacity: 0.7, background: "#FF8899" },
            close: function () {
            },
            buttons: {
                '確認': function () {
                    if ($("#StoreId").val() == "" || $("#CategoryId").val() == "")
                        alert("請選擇欲更改的店家");
                    else {
                        storeId = $("#StoreId").val();
                        $.ajax({
                            url: "/shb/Orders/Get_StoreInfo",
                            data: { storeId: storeId },
                            type: "POST",
                            success: function (data) {
                                $.each(data, function (key, val) {
                                    if (key == "StoreId") {
                                        $("#storeid").text(val);
                                        $("#storeid_hidden").val(val);
                                        $("#storeid_hidden_change").val("1");
                                    }
                                    else if (key == "StoreName")
                                        $("#storename").text(val);
                                    else if (key == "SendTitle")
                                        $("#sendtitle").text(val);
                                    else if (key == "SendName")
                                        $("#sendname").text(val);

                                });
                                //will change some text on page
                                //alert(data);
                                $("#StoreId").html(data);
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                alert(xhr.status);
                                alert(thrownError);
                            }
                        });
                        $("#storetitle").text($("#CategoryId :selected").text() + " " + $("#StoreId :selected").text());
                        $(this).dialog('close');
                    }
                },
                '關閉': function () {
                    $(this).dialog('close');
                }
            }
        });
        //}
    });
    var getcid = $('#CategoryId').val();
    var val = $("#StoreId").attr("data-id");
    $('#CategoryId').change(function () {
        getcid = $('#CategoryId').val();
        ajax_stores(getcid);
    });
    ajax_stores(getcid);
    checkfun("#invoice_0", ".invoice_0");
    checkfun("#invoice_1", ".invoice_1");
    checkfun("#invoice_2", ".invoice_2");
    var Reg = /^[a-zA-z]+\d{8,8}$/
    $(".invoice_error2").hide();
    $(".invoice_success").hide();
    $(".invoice_error").hide().css("cursor", "pointer");
    var invoice_count = $(".invoice_error:last").attr("data-value");
    for (var i = 0; i <= invoice_count; i++) {
        $("#InvoiceNumber[data-value='" + i + "']").focusout(function () {
            var id = ajax_checkInvoice($(this).val(), $(this).attr("data-id"));
            if (!Reg.test($("#InvoiceNumber").val())) {
                $(this).parent().next().next().next(".invoice_error2").attr("Title", "發票格式錯誤").show();
                $(this).parent().next().next(".invoice_error").hide();
                $(this).parent().next(".invoice_success").hide();

            } else {
                if (id == "-1") {
                    $(this).parent().next().next(".invoice_error").hide();
                    $(this).parent().next().next().next(".invoice_error2").hide();
                    $(this).parent().next(".invoice_success").show();
                } else {

                    $(this).parent().next().next(".invoice_error").attr({ "data-id": id, "Title": "重複發票，查看重複發票" }).show();
                    $(this).parent().next().next().next(".invoice_error2").hide();
                    $(this).parent().next(".invoice_success").hide();
                }
            }
        });
    }
    $(".invoice_error").click(function () {
        window.open("/shb/Invoice/Edit/" + $(this).attr("data-id"));
    })
    $(".ajax_delete").click(function () {
        if (confirm("確認刪除這筆生日?")) {
            var dateString = $("select[name=year][data-count='" + $(this).attr("data-count") + "']").val() + "/" +
               $("select[name=month][data-count='" + $(this).attr("data-count") + "']").val() + "/" +
               $("select[name=date][data-count='" + $(this).attr("data-count") + "']").val();
            var vid = $("#Id").val();
            var babyid = $(this).attr("data-val");
            //var babyid = $("#babyid").val;
            $.ajax({
                type: "post",
                url: RootPath + "/Volunteers/Ajax_DeleteBaby",
                data: { BabyValue: dateString, vid: vid, babyid: babyid },
                dataType: "json",
                //jsonp: "callback",
                //jsonpCallback: "dool",
                complete: function (data) {
                    location.reload();
                }
            });
        }
        else {
        }
    });

    $("#store_list").change(function () {
        $.ajax({
            type: "post",
            traditional: true,
            async: true,
            url: RootPath + "/Home/Ajax_Store",
            data: { cid: $(this).val() },
            dataType: "json",
            //jsonp: "callback",
            //jsonpCallback: "dool",
            success: function (data) {
                if (data) {
                    var temp = "<option>請選擇</option>";
                    for (var i in data) {
                        temp += "<option value=\"" + data[i].Id + "\">" + data[i].Title + "</option>";
                    }
                    $("#StoreId").html(temp);
                    $("#StoreId").dropkick('refresh');
                    if (data.length > 0) {
                        $("#dk_container_StoreId").children('a.dk_toggle').width($("#dk_container_StoreId").children('a.dk_toggle').width() - 20);
                    }
                }
            }
        });
    });
    $(".invoice_check").click(function () {
        var val = $(this).prop("checked");
        $(this).parent().parent().parent().find(".invoice_check").each(function () {
            $(this).prop("checked", false);
        });
        $(this).prop("checked", val);
    });
    $(".invoice_title").click(function () {
        var val = $(this).prop("checked");
        $(this).parent().parent().parent().find(".invoice_title").each(function () {
            $(this).prop("checked", false);
        });
        $(this).prop("checked", val);
    });
    $("img.preview").kabbar();
    $(".status").change(function () {
        if ($("#status1").is(":checked")) {
            $("input#email_confirm[dataid='0']").prop("checked", true);
            $("#email_confirm").prop("hidden", true);
            $("#failreason").prop("hidden", true);
        }
        else if ($("#status2").is(":checked")) {
            $("input#email_confirm[dataid='1']").prop("checked", true);
            $("#email_confirm").prop("hidden", false);
            $("#failreason").prop("hidden", true);
        }
        else if ($("#status3").is(":checked")) {
            $("input#email_confirm[dataid='2']").prop("checked", true);
            $("#email_confirm").prop("hidden", false);
            $("#failreason").prop("hidden", false);
            $(".FR").removeAttr("disabled");
        }

    });
    $("#Enable").click(function () {
        if ($('#Enable').prop('checked')) {
            $("input#email_confirm[dataid='1']").prop("checked", true);
            $("input#Review").prop("checked", true);
            $(".FR").attr("disabled", "disabled");
            $(".FR").prop("checked", false);
        } else {
            $("input#email_confirm[dataid='2']").prop("checked", true);
            $("input#Review").prop("checked", true);
            $(".FR").removeAttr("disabled");
        }
    });
    $("input#email_confirm[dataid='0']").click(function () {
        $(".FR").attr("disabled", "disabled");
        $(".FR").prop("checked", false);
    });
    $("input#email_confirm[dataid='2']").click(function () {
        $("#Enable").prop("checked", false);
        $("input#Review").prop("checked", true);
        $(".FR").removeAttr("disabled");
    });
    $("input#email_confirm[dataid='1']").click(function () {
        $("#Enable").prop("checked", true);
        $("input#Review").prop("checked", true);
        $(".FR").prop("checked", false);
        $(".FR").attr("disabled", "disabled");
    });
    var amount = $(".default:last").attr("data-count");

    for (var i = 0; i <= amount; i++) {
        var d = new Date();
        var year = d.getFullYear();
        var monthNow = d.getMonth();
        var month = $(".default[name=month][data-count='" + i + "']").attr("data-val");
        //year
        ($(".default[name=year][data-count='" + i + "']").attr("data-val") == null) ? $(".default[name=year][data-count='" + i + "']").append(Get_Number(year - 7, year + 1, false)) :
        $(".default[name=year][data-count='" + i + "']").append(Get_Number(year - 7, year + 1, $(".default[name=year][data-count='" + i + "']").attr("data-val")));
        //month
        (month == null) ? $(".default[name=month][data-count='" + i + "']").append(Get_Number(1, 12, false)) :
        $(".default[name=month][data-count='" + i + "']").append(Get_Number(1, 12, $(".default[name=month][data-count='" + i + "']").attr("data-val")));
        //day
        var y = parseInt($("select[name=year] option:selected").val(), 10);
        var m = parseInt($("select[name=month] option:selected").val(), 10);
        ($(".default[name=date][data-count='" + i + "']").attr("data-val") == null) ?
        $(".default[name=date][data-count='" + i + "']")
            .append(Get_Number(1, Get_Days(y, m), false)) :
        $(".default[name=date][data-count='" + i + "']")
            .append(Get_Number(1, Get_Days(y, m), $(".default[name=date][data-count='" + i + "']").attr("data-val")));
        //onchange
        $(".default[name=year][data-count='" + i + "']").change(function () {
            $(this).next().empty();
            $(this).next().append(Get_Number(1, 12, false));
            $(this).next().next().empty();
            $(this).next().next().append(Get_Number(1, Get_Days(y, m), false));
        });
        $(".default[name=month][data-count='" + i + "']").change(function () {
            y = parseInt($(this).prev().val(), 10);
            m = parseInt($(this).val(), 10);
            $(this).next().empty();
            $(this).next().append(Get_Number(1, Get_Days(y, m), false));
        });
    }



    //$(".default[name=month][data-count='" + i + "']").

    //$(".default").dropkick();
    if (isNaN(amount)) {
        var d = new Date();
        var year = d.getFullYear();
        addcount = 0;
        amount = 1;
        $("#baby_div").append("<label for=\"name\" class=\"name\" data-id=\"1\"><span class=\"required\">*</span>寶寶 " + amount + " 生日  </label>").append("<div class=\"clearfix\"><select name=\"year\" data-count=0 class=\"default\" data-val=\"\" tabindex=\"\"></select><select name=\"month\" data-count=0 class=\"default\" data-val=\"\" tabindex=\"\"></select><select name=\"date\" data-count=0 class=\"default\" data-val=\"\" tabindex=\"\"></select></div><hr class=\"clear\" style=\"display:none\" />");
        $(".default[name=year][data-count='0']").append(Get_Number(year - 7, year + 1, false));
        $(".default[name=month][data-count='0']").append(Get_Number(1, 12, false));
        var y = parseInt($("select[name=year] option:selected").val(), 10);
        var m = parseInt($("select[name=month] option:selected").val(), 10);
        $(".default[name=date][data-count='0']").append(Get_Number(1, Get_Days(y, m), false));

        //havebug need to edit
        $(".default[name=year][data-count='0']").change(function () {
            $(this).next().empty();
            $(this).next().append(Get_Number(1, 12, false));
            $(this).next().next().empty();
            $(this).next().next().append(Get_Number(1, Get_Days(y, m), false));
        });
        $(".default[name=month][data-count='0']").change(function () {
            y = parseInt($(this).prev().val(), 10);
            m = parseInt($(this).val(), 10);
            $(this).next().empty();
            $(this).next().append(Get_Number(1, Get_Days(y, m), false));
        });
    }

    $("#addBaby").click(function () {
        amount = $(".name").last().attr("data-id");
        if (isNaN(amount))
            amount = 0;
        addcount = parseInt(amount, 10) + 1;
        $("#baby_div").append("</br><label for=\"name\" class=\"name\" data-id=" + addcount + " ><span class=\"required\">*</span>寶寶 " + addcount + " 生日  </label>").append("<div class=\"editor-field\"><select name=\"year\" data-count=" + addcount + " class=\"default\" data-val=\"\" tabindex=\"\"></select><select name=\"month\" data-count=" + addcount + " class=\"default\" data-val=\"\" tabindex=\"\"></select><select name=\"date\" data-count=" + addcount + " class=\"default\" data-val=\"\" tabindex=\"\"></select></div>");
        var paddingTop = parseInt($(this).css("padding-top"), 10);
        $(".default[name=year][data-count='" + addcount + "']").append(Get_Number(year - 7, year + 1, false));
        $(".default[name=month][data-count='" + addcount + "']").append(Get_Number(1, 12, false));
        $(".default[name=date][data-count='" + addcount + "']").append(Get_Number(1, Get_Days(y, m), false));
        amount++;

        for (var i = amount; i <= addcount; i++) {
            $(".default[name=year][data-count='" + i + "']").change(function () {
                $(this).next().empty();
                $(this).next().append(Get_Number(1, 12, false));
                $(this).next().next().empty();
                $(this).next().next().append(Get_Number(1, Get_Days(y, m), false));
            });
            $(".default[name=month][data-count='" + i + "']").change(function () {
                y = parseInt($(this).prev().val(), 10);
                m = parseInt($(this).val(), 10);
                $(this).next().empty();
                $(this).next().append(Get_Number(1, Get_Days(y, m), false));
            });
        }
    });

    $("form").submit(function () {
        if ($("input#email_confirm[dataid='2']").prop("checked"))
            if (isNaN($("input[name=FailReason]:checked").val())) {
                alert("請選擇審核失敗原因");
                return false;
            }
        return true;
    });
});
function checkfun(id, cls) {
    $(id).click(function () {
        $(".invoice_check").each(function () {
            $(this).prop("checked", false);
        });
        $(cls).each(function () {
            if ($(id).prop("checked"))
                $(this).prop("checked", $(id).prop("checked"));
        });
    });
}
function ajax_checkInvoice(invoicenumber, id) {

    var jqXHR = $.ajax({
        type: "post",
        traditional: true,
        async: false,
        url: RootPath + "/Member/Ajax_Invoices",
        data: { invoicenumber: invoicenumber, id: id },
        success: function (data) {
        }
    });
    return jqXHR.responseText;
}
function Get_Number(start, end, select) {
    var selected = "selected=\"selected\"";
    var html = "<option value=\"\" " + (select == 0 ? selected : "") + ">請選擇</option>";
    for (var i = start; i <= end; i++) {
        html += "<option value=\"" + i + "\" " + (select == i ? selected : "") + ">" + i + "</option>";
    }
    return html;
}
function Get_Days(y, m) {
    if (y % 4 == 0 && m == 2) {
        return 29;
    }
    if (m == 2) {
        return 28;
    }
    if (m == 1 || m == 3 || m == 5 || m == 7 || m == 8 || m == 10 || m == 12) {
        return 31;
    }
    if (m == 4 || m == 6 || m == 9 || m == 11) {
        return 30;
    }
    return 0;
}
function ajax_stores(getcid) {
    $('#StoreId').empty();
    $.ajax({
        type: "POST",
        url: "/shb/Invoice/Ajax_Stores",
        dataType: "JSON",
        data: { cid: getcid },
        success: function (response) {
            $('#StoreId').append("<option>請選擇</option>");
            for (var i = 0; i < response.count; i++)
                $('#StoreId').append("<option id=StoreItem value=" + response.Store[i].Id + " data-id=" + response.Store[i].Id + ">" + response.Store[i].Title + "</option>");

            $("#StoreItem[data-id='" + $('#StoreId').attr('data-id') + "']").attr("selected", "selected");
        }
    });
}