$(function () {
    checkfun("#invoice_0", ".invoice_0");
    checkfun("#invoice_1", ".invoice_1");
    checkfun("#invoice_2", ".invoice_2");
    $(".invoice_success").hide();
    $(".invoice_error").hide();
    $(".invoice_error2").hide();
    $("#InvoiceNumber").focusout(function () {
        var id = ajax_checkInvoices($(this).val(), $(".invoice_error").attr("data-id"));
        var Reg = /^[a-zA-z]+\d{8,8}$/
        if (!Reg.test($("#InvoiceNumber").val())) {
            $(".invoice_error2").show();
            $(".invoice_success").hide();
            $(".invoice_error").hide();
        } else {
            if (id == "-1") {
                $(".invoice_success").show();
                $(".invoice_error").hide();
                $(".invoice_error2").hide();
            } else {
                $(".invoice_error").show().css("cursor", "pointer");
                $(".invoice_success").hide();
                $(".invoice_error2").hide();
            }
        }
    });
    $(".invoice_error").click(function () {
        var id = ajax_checkInvoice($("#InvoiceNumber").val())
        window.open("/shb/Invoice/Edit/" + id);
    })
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
    
    $("#Enable").click(function () {
        if ($('#Enable').prop('checked')) {
            $("input#email_confirm[dataid='1']").prop("checked", true);
        } else {
            $("input#email_confirm[dataid='2']").prop("checked", true);
        }
    });

    $(".Identify").change(function () {
        if ($("#Identify1").is(":checked"))
        {
            $("#SerialNo").hide();
        }
        if($("#Identify2").is(":checked"))
        {
            $("#SerialNo").show();
        }
    });
});

function ajax_checkInvoices(invoicenumber, id) {

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
function ajax_checkInvoice(invoicenumber) {

    var jqXHR = $.ajax({
        type: "post",
        traditional: true,
        async: false,
        url: RootPath + "/Member/Ajax_Invoice",
        data: { invoicenumber: invoicenumber},
        success: function (data) {
        }
    });
    return jqXHR.responseText;
}
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