$(function () {
    $(".StatesOption").change(function () {
        document.getElementById("OrdersStatesTemp").value = this.value
    });
    $(".ProductOption").change(function () {
        document.getElementById("ProductTemp").value = this.value
    });

    $(".ExchangeGiftStates").change(function () {
        if (document.getElementById("Update_Id").value == "") {
            document.getElementById("Update_Id").value = $(this).attr('id');
            document.getElementById("Update_State").value = this.value;
        } else {
            document.getElementById("Update_Id").value = document.getElementById("Update_Id").value + "," + $(this).attr('id');
            document.getElementById("Update_State").value = document.getElementById("Update_State").value + "," + this.value;
        }
    });
});