const _url = {};
$(function () {
    setValidation();
    _url.get_t_reale_CompanyInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyInfo';
    _url.get_t_reale_CompanyCountingInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyCountingInfo';
    _url.get_t_reale_purchase_DisplayData = common.appPath + '/t_reale_account/get_t_reale_account_DisplayData'; 

    _url.get_t_reale_purchase_CSVData = common.appPath + '/t_reale_account/get_t_reale_account_DisplayData';
    _url.Insert_L_Log = common.appPath + '/t_reale_purchase/Insert_L_Log';
    _url.get_Modal_HomeData = common.appPath + '/t_reale_purchase/get_Modal_HomeData';
    _url.get_Modal_ProfileData = common.appPath + '/t_reale_purchase/get_Modal_ProfileData';
    _url.get_Modal_ContactData = common.appPath + '/t_reale_purchase/get_Modal_ContactData';
    _url.get_Modal_DetailData = common.appPath + '/t_reale_purchase/get_Modal_DetailData';
    addEvents();
    $('#subMenu li').children('a').removeClass("active");
    $('#subMenu li').children('a').eq(6).addClass('active');
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_account').addClass('font-bold text-underline');
});

function setValidation() {
    //$('.form-areamansion')
    //    .addvalidation_errorElement("#CheckBoxError")
    //    .addvalidation_checkboxlenght(); //E112

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_reqired() // E101 
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_reqired() // E101
        .addvalidation_datecompare(); //E111

    $('#penaltyArea').addvalidation_errorElement("#errorpenalty").addvalidation_reqired();

}

function addEvents() {
    common.bindValidationEvent('#form1', '');
    const btn = document.querySelector("#flexSwitchCheckDefault_Penalty")
    btn.addEventListener("change", function () {
        if (this.checked) {
            $('.disabled-account').removeAttr("disabled")
            $('.cap-errormsg-right').removeClass("d-none")
        }
        else {
            $('.disabled-account').attr("disabled", "true").removeClass("cap-is-invalid")
            $('.cap-errormsg-right').addClass("d-none")
        }

    });
    document.querySelector("#flexSwitchCheckDefault_Penalty").checked ? $('.disabled-account').removeAttr("disabled") : $('.disabled-account').attr("disabled", "true")
    $('#StartDate, #EndDate').on('change', function () {
        const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val();
        if (!common.checkValidityInput($this)) { 
            return false;
        } 
        let model = {
            StartDate: $start,
            EndDate: $end
        };
        if (model.StartDate && model.EndDate) {
            if (model.StartDate < model.EndDate) {
                $("#StartDate").hideError();
                $("#EndDate").hideError();
                $("#EndDate").focus();
                return;
            }
        }
    });
    $('#subMenu li').children('a').on('click', function () {
        $('#subMenu li').children('a').removeClass("active");
        $(this).addClass('active');
    });
    let model = {
        RealECD: common.getUrlParameter('reale') 
    };

    $('#seller').addClass('d-none');
    $('#submenu_seller').addClass('d-none');

    Bind_Company_Data(this);
    //Bind Company Info Data to the title part of the page

    get_purchase_Data(model, this, 'page_load');

   // sortTable.getSortingTable("tblPurchaseDetails"); 
     
}

function get_purchase_Data(model, $form, state) { 
    common.callAjaxWithLoading(_url.get_t_reale_purchase_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            BindPenalty(result.data.split('Ʈ')[0]);
            Bind_DisplayData(result.data.split('Ʈ')[1]);
            //if (state == 'Display')
            //    l_logfunction(model.RealECD + ' ' + $('#r_REName').text(), 'display', '');
        }

        if (result && !result.isOK) {
            // alert(result.data);
            const errors = result.data;
            for (key in errors) {
                //alert(errors.count)
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

function Bind_DisplayData(result) {
     
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {


            html += 
              '<div class="col-12 border-1 m-1">\
                <strong>'+ data[i]['PenaltyStartDate'] + '～' + data[i]['PenaltyEndDate'] + '</strong >\
                    <p>'+ data[i]['PenaltyMemo'] +'</p>\
                    <div class="float-end">\
                        <p>登録スタッフ：<p>'+ data[i]['TenStaffName'] +'</p>\
                        </p>\
                    </div>\
                                                    </div>'
        }
    }
    $('#penaltyList').append(html);
}
function BindPenalty(result) {
    let data = JSON.parse(result);
    $('#flexSwitchCheckDefault').checked = data[0]['TestFlg'];
    $('#flexSwitchCheckDefault_Penalty').checked = data[0]['PenaltyFlg'];
    $('#StartDate').val(data[0]['PenaltyStartDate']);
    $('#EndDate').val(data[0]['PenaltyEndDate']);
    $('#penaltyArea').val(data[0]['PenaltyMemo']);
}
function l_logfunction(remark, Process, id) {

    let model = {
        LoginKBN: '3',
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: 't_reale_asmhis',
        Processing: Process,
        Remarks: remark,
    };
    common.callAjax(_url.Insert_L_Log, model, function (result) {
        if (result && result.isOK) {
            if (remark == 't_seller_assessment ' + id) {
                window.location.href = window.location.href.replace('t_reale_asmhis', 't_seller_assessment') + "&SellerCD=" + id;
            }
            else if (remark == 't_seller_assessment_detail ' + id) {
                window.location.href = window.location.href.replace('t_reale_asmhis', 't_seller_assessment_detail') + "&AssReqID=" + id;
            }
        }
    });
}
