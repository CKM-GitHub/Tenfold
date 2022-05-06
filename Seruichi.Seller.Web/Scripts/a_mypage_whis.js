const _url = {};

$(function () {

    setValidation();
    _url.getM_SellerList = common.appPath + '/a_mypage_whis/GetD_SellerPossibleData';
    addEvents();
});

function setValidation() {

    $('#SellerCD')
        .addvalidation_errorElement("#errorSeller")
        .addvalidation_maxlengthCheck(10);

    $('#PossibleID')
        .addvalidation_errorElement("#PossibleID")


    $('#ChangeDateTime')
        .addvalidation_errorElement("#ChangeDateTime")
        .addvalidation_datecheck()
        .addvalidation_datecompare();

    $('#ChangeCount')
        .addvalidation_errorElement("#ChangeCount")
    

    $('#ChangeFee')
        .addvalidation_errorElement("#ChangeFee")

    $('#PaymentFlg')
        .addvalidation_errorElement("#PaymentFlg")
}
function addEvents() {

    
    common.bindValidationEvent('#form1', '');

  

    const $PossibleID = $("#PossibleID").val(), $SellerCD = $("#SellerCD").val(), $ChangeDateTime = $('#ChangeDateTime').text(),
        $ChangeCount = $("#ChangeCount").val(), $ChangeFee = $("#ChangeFee").val(), $PaymentFlg = $("#PaymentFlg").val()

    let model = {
        PossibleID: $PossibleID,
        SellerCD: $SellerCD,
        ChangeDateTime: $ChangeDateTime,
        ChangeCount: $ChangeCount,
        ChangeFee: $ChangeFee,
        PaymentFlg: $PaymentFlg,
    };
    getM_SellerList(model, this);

    
}
function getM_SellerList(model, $form) {
    common.callAjaxWithLoading(_url.getM_SellerList, model, this, function (result) {
        if (result && result.isOK) {

            Bind_tbody(result.data);
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
}
function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}
function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    let _letter = "";
    let _class = "";
    let _sort_checkbox = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            if (isEmptyOrSpaces(data[i]["ステータス"])) {
                _letter = "";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
                _sort_checkbox = "";
            }
            //else {
            //    _letter = data[i]["ステータス"].charAt(0);
            //    if (_letter == "見") {
            //        _class = "ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal";
            //        _sort_checkbox = "t_seller_list_one";
            //    }
            //    else if (_letter == "交") {
            //        _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark fst-normal";
            //        _sort_checkbox = "t_seller_list_two";
            //    }
            //    else if (_letter == "終") {
            //        _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary fst-normal";
            //        _sort_checkbox = "t_seller_list_three";
            //    }

            //}
            html += '<tr>\
            <td class= "text-end" > ' + + '</td>\
            <td ' + data[i]["PossibleID"] + '</span></td >\
            <td class="text-center"> ' + data[i]["ChangeDateTime"] + ' </td>\
            <td> ' + data[i]["ChangeCountCD"] + ' </td>\
            <td> '+ data[i]["ChangeFeeCD"] + '</td>\
            <td> ' + data[i]["PaymentFlg"] + ' </td>\</tr>'
           
            
        }

        $('#total_record').text("検索結果：" + data.length + "件")
        $('#total_record_up').text("検索結果：" + data.length + "件")
    }
    else {
        $('#total_record').text("検索結果： 0件")
        $('#total_record_up').text("検索結果： 0件")
    }
    $('#mansiontable tbody').append(html);

    sortTable.getSortingTable("mansiontable");
}

function l_logfunction(id) {
    let model = {
        PossibleID: null,
        ChangeDateTime: null,
        ChangeCount: null,
        ChangeFee: null,
        PaymentFlg: null,
        SellerCD: id
    }
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
                window.location.href = common.appPath + '/a_mypage_whis/Index?SellerCD=' + model.SellerCD;
                //window.location.href = '/t_seller_assessment/Index';
                //alert("https://www.seruichi.com/t_seller_assessment?seller" + model.SellerCD);
            }
            if (result && !result.isOK) {

            }
        });
}

