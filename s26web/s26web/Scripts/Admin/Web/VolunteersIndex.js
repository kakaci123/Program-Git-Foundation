function exportexcel() {
    document.cond.action = "/shb/Volunteers/Export_main";
    document.cond.method = "post";
    cond.submit();
};
function searchdata() {
    document.cond.action = "/shb/Volunteers";
    document.cond.method = "get";
    cond.submit();
};
$(document).ready(function () {
    $("img.preview").kabbar();
    $("img.preview").click(function () {
        $("#__RitrattKabbari__").transition({ rotate: '+=90' });
    });
    $("#time_offset").val(new Date().getTimezoneOffset());

    getcid = $('#CategoryId').val();
    ajax_stores(getcid);

    $('#CategoryId').change(function () {
        getcid = $('#CategoryId').val();
        ajax_stores(getcid);
    });

});

function ajax_stores(getcid) {
    var select = $('#StoreId').attr("data-id");
    $('#StoreId').empty();
    $.ajax({    
        type: "POST",
        url: "/Invoice/Ajax_Stores",
        dataType: "JSON",
        data: { cid: getcid },
        success: function (response) {
            $('#StoreId').append("<option>請選擇</option>");
            for (var i = 0; i < response.count; i++) {
                if (select == response.Store[i].Id) {
                    $('#StoreId').append("<option id=StoreItem value=" + response.Store[i].Id + " data-id=" + response.Store[i].Id + " selected>" + response.Store[i].Title + "</option>");
                }
                else {
                    $('#StoreId').append("<option id=StoreItem value=" + response.Store[i].Id + " data-id=" + response.Store[i].Id + " >" + response.Store[i].Title + "</option>");
                }
            }
            $("#StoreItem[data-id='" + $('#StoreId').attr('data-id') + "']").attr("selected", "selected");
        }
    });
}
