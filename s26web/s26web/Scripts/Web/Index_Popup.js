$(function () {
    //if ($("#error").attr("data-val") != null) {
    //    alert($("#error").attr("data-val"));
    //}
});
function vaild() {
    var alert_text = "";
    
    var form_name = document.getElementById('login');
    for (var i = 0; i < form_name.male.length; i++) {
        if (form_name.male[i].checked) {
            break;
        }
    }

    if ($(".default[name=date]").val() == null | $(".default[name=date]").val() == "") {
        alert_text += "請輸入寶寶生日\n";
    }

    if (form_name.male[i].value == 4) {
        if ($("#box_01").val() == "") {
            alert_text += "請輸入品牌名稱\n";
        }
    }

    if (alert_text!="") {
        alert(alert_text);
        return false;
    }
}