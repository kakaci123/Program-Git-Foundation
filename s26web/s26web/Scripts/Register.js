$(function () {
    if ($("#error").attr("data-val") != null) {
        alert($("#error").attr("data-val"));
    }
});
function vaild() {
    var alert_text = "";
    var re = /\.(jpg|gif)$/i;  //允許的圖片副檔名

    if ($(".default[name=date]").val() == null | $(".default[name=date]").val() == "") {
        alert_text += "請輸入寶寶生日\n";
    }

    var form_name = document.getElementById('register_from');
    for (var i = 0; i < form_name.male.length; i++) {
        if (form_name.male[i].checked) {
            break;
        }
    }
    if (form_name.male[i].value == 4) {
        if ($("#box_Product").val() == "") {
            alert_text += "請輸入品牌名稱\n";
        }
    }

    if (document.getElementById('rd_1').checked) {

        if ($("#id_image_large").val() == "") {
            alert_text += "請選擇照片\n";
        }
  
        if (!re.test($("#id_image_large").val())) {
            alert_text += "只允許上傳JPG或GIF影像檔\n";
        }

    } else if (document.getElementById('rd_2').checked) {
        var txt_input = document.getElementById('box_ok').value;
        if (txt_input.trim() == "") {
            alert_text += "請輸入促銷代碼\n";
        }
    }

    if ($('#agree1').prop("checked") && $('#agree2').prop("checked")) {
    }
    else {
        alert_text += "請閱讀及確認勾選相關會員權益事項\n";
    }

    if (alert_text!="") {
        alert(alert_text);
        return false;
    }
}