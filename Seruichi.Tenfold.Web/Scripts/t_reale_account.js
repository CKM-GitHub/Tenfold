const _url = {};
$(function () {
    setValidation();
    _url.get_t_reale_CompanyInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyInfo';
    _url.get_t_reale_CompanyCountingInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyCountingInfo';
    _url.get_t_reale_purchase_DisplayData = common.appPath + '/t_reale_account/get_t_reale_account_DisplayData';

    _url.get_t_reale_accountSave = common.appPath + '/t_reale_account/get_t_reale_account_ManipulateData';
    _url.Insert_L_Log = common.appPath + '/t_reale_account/Insert_L_Log';
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
    $('#seqPenalty').val('-1');
    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_reqired() // E101 
    //.addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_reqired() // E101
        .addvalidation_datecompare(); //E111

    $('#penaltyArea').addvalidation_errorElement("#errorpenalty").addvalidation_reqired();

}
function Date_Compare() {
    $start = $('#StartDate').val(), $end = $('#EndDate').val()
    let model = {
        StartDate: $start,
        EndDate: $end
    };
    if (model.StartDate && model.EndDate) {
        if (model.StartDate <= model.EndDate) {
            $("#StartDate").hideError();
            $("#EndDate").hideError();
            return;
        }
        else {
            $("#StartDate").showError(common.getMessage('E111'));
            $("#EndDate").showError(common.getMessage('E111'));
            return;
        }
    }
}
function SetEmpty() {

}
function addEvents() {
    common.bindValidationEvent('#form1', '');

    const btn = document.querySelector("#flexSwitchCheckDefault_Penalty")
    btn.addEventListener("change", function () {
        if (this.checked) {
            $('.disabled-account').removeAttr("disabled")
            //$('.cap-errormsg-right').removeClass("d-none")
        }
        else {
            $('.disabled-account').attr("disabled", "true").removeClass("cap-is-invalid")
            //$('.cap-errormsg-right').addClass("d-none")
            // $('.cap-errormsg-right').addClass("d-none")
            $('#StartDate').val(null);
            $('#EndDate').val(null);
            $('#penaltyArea').val(null);
            staticCache.StartDate = $('#StartDate').val();
            staticCache.EndDate = $('#EndDate').val();
            staticCache.Memo = $('#penaltyArea').val();
            $("#StartDate").hideError();
            $("#EndDate").hideError();
            $("#penaltyArea").hideError();


        }


    });
    $('#StartDate, #EndDate').on('change', function () {
        // Date_Compare();
        //const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val();
        //if (!common.checkValidityInput($this)) { 
        //    return false;
        //} 
        //let model = {
        //    StartDate: $start,
        //    EndDate: $end
        //};
        //if (model.StartDate && model.EndDate) {
        //    if (model.StartDate < model.EndDate) {
        //        $("#StartDate").hideError();
        //        $("#EndDate").hideError();
        //        $("#EndDate").focus();
        //        return;
        //    }
        //}
        Date_Compare();

        //const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val();
        //if (!common.checkValidityInput($this)) {
        //    return false;
        //}
        //let model = {
        //    StartDate: $start,
        //    EndDate: $end
        //};
        //if (model.StartDate && model.EndDate) {
        //    if (model.StartDate < model.EndDate) {
        //        $("#StartDate").hideError();
        //        $("#EndDate").hideError();
        //        //$("#EndDate").focus();
        //        return;
        //    }
        //    else {
        //        $("#StartDate").showError(common.getMessage('E111'));
        //        $("#EndDate").showError(common.getMessage('E111'));
        //        return;
        //    }
        //}
    });
    $('#subMenu li').children('a').on('click', function () {
        $('#subMenu li').children('a').removeClass("active");
        $(this).addClass('active');
    });

    $('#btn_save').on('click', function () {


        SetCache();
        $('#modal-changesave').modal('hide');
        common.callAjaxWithLoadingSync(_url.get_t_reale_accountSave, staticCache, this, function (result) {
            if (result && result.isOK) {
                SetDefault();
                l_logfunction();
                $('#modal-changeok').modal('show');
                //#modal-changeok
            }
            else {
                //alert('Errors')
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                    this.getInvalidItems().get(0).focus();
                    return;
                }
            }
        })
    });
  
     
    $('#wrapper').prop('style', 'padding-left:0px !important')
    SetDefault();
    //let model = {
    //    RealECD: common.getUrlParameter('RealECD')
    //};
    //get_purchase_Data(model, this, 'page_load');

    //document.querySelector("#flexSwitchCheckDefault_Penalty").checked ? $('.disabled-account').removeAttr("disabled") : $('.disabled-account').attr("disabled", "true")

    //SetCache();
}

function SetDefault() {
    clearBack();
    $('#seqPenalty').val('-1');
    let model = {
        RealECD: common.getUrlParameter('RealECD')
    };
    get_purchase_Data(model, this, 'page_load');
    document.querySelector("#flexSwitchCheckDefault_Penalty").checked = false;
    document.querySelector("#flexSwitchCheckDefault_Penalty").checked ? $('.disabled-account').removeAttr("disabled") : $('.disabled-account').attr("disabled", "true")
    SetCache();
}

var staticCache = {
    penaltyFlg: '',
    testFlg: '',
    StartDate: '',
    EndDate: '',
    Memo: '',
    RealECD: '',
    Isfake: '1',
    SEQPenalty:'-1'
};
function CheckChanges() {
    $form = $('#form1').hideChildErrors();
    if (!common.checkValidityOnSave('#form1')) {
        $form.getInvalidItems().get(0).focus();
        return false;
    }
    //debugger
    var currrentState = {
        penaltyFlg: document.querySelector("#flexSwitchCheckDefault_Penalty").checked ? '1' : '0',
        testFlg: document.querySelector("#flexSwitchCheckDefault").checked ? '1' : '0',
        StartDate: $('#StartDate').val(),
        EndDate: $('#EndDate').val(),
        Memo: $('#penaltyArea').val(),
        RealECD: common.getUrlParameter('RealECD'),
    }
    $('#chagesCheck').removeAttr('data-bs-target');
    $('#chagesCheck').removeAttr('data-bs-toggle');

    if (staticCache.StartDate === currrentState.StartDate && staticCache.EndDate === currrentState.EndDate && staticCache.Memo === currrentState.Memo && staticCache.testFlg === currrentState.testFlg && staticCache.penaltyFlg === currrentState.penaltyFlg) {
        $('#modal-changesave').modal('hide');

    }
    else {
        if (staticCache.StartDate === currrentState.StartDate && staticCache.EndDate === currrentState.EndDate && staticCache.Memo === currrentState.Memo && staticCache.penaltyFlg === currrentState.penaltyFlg) {
            staticCache.Isfake = '0';
        }
        else
            staticCache.Isfake = '1';

        $('#modal-changesave').modal('show');
    }
}
function discardChanges() {
   
    SetDefault();
    //let model = {
    //    RealECD: common.getUrlParameter('RealECD')
    //};
    //get_purchase_Data(model, this, 'page_load');
    ////$('#flexSwitchCheckDefault_Penalty').attr("checked", "false");
    //document.querySelector("#flexSwitchCheckDefault_Penalty").checked = false;
    //document.querySelector("#flexSwitchCheckDefault_Penalty").checked ? $('.disabled-account').removeAttr("disabled") : $('.disabled-account').attr("disabled", "true")
    //SetCache();
    //common.callAjaxWithLoadingSync(_url.get_t_reale_purchase_DisplayData, staticCache, this, function (result) {
    //    if (result && result.isOK) {
    //        //alert('hi')
    //        $("#StartDate").hideError();
    //        $("#EndDate").hideError();
    //        $("#penaltyArea").hideError();
    //        $('.cap-errormsg-right').removeClass("d-none")
    //        let data = JSON.parse(result.data.split('Ʈ')[0]);
    //        if (data.length) {
                //if (data[0]['TestFlg'] == '1') {
                //    $('#flexSwitchCheckDefault').attr("checked", "true");
                //    document.querySelector("#flexSwitchCheckDefault").checked = true;

                //}
                //else {
                //    $('#flexSwitchCheckDefault').removeAttr('checked')
                //    document.querySelector("#flexSwitchCheckDefault").checked = false;
                //}
                //if (data[0]['PenaltyFlg'] == '1') {
                //    $('#flexSwitchCheckDefault_Penalty').attr("checked", "true");
                //    document.querySelector("#flexSwitchCheckDefault_Penalty").checked = true;
                //}
                //else {
                //    $('#flexSwitchCheckDefault_Penalty').removeAttr('checked')
                //    document.querySelector("#flexSwitchCheckDefault_Penalty").checked = false;
                //}
                //$('#StartDate').val(data[0]['PenaltyStartDate']);
                //$('#EndDate').val(data[0]['PenaltyEndDate']);
                //$('#penaltyArea').val(data[0]['PenaltyMemo']);
                //document.querySelector("#flexSwitchCheckDefault_Penalty").checked ? $('.disabled-account').removeAttr("disabled") : $('.disabled-account').attr("disabled", "true")

             
        //    }
             
        //}
    //})
}

function SetCache(id) {
    staticCache.penaltyFlg = document.querySelector("#flexSwitchCheckDefault_Penalty").checked ? '1' : '0';
    staticCache.testFlg = document.querySelector("#flexSwitchCheckDefault").checked ? '1' : '0';
    staticCache.StartDate = $('#StartDate').val();
    staticCache.EndDate = $('#EndDate').val();
    staticCache.Memo = $('#penaltyArea').val();
    staticCache.RealECD = common.getUrlParameter('RealECD');
    staticCache.SEQPenalty = $('#seqPenalty').val();
    // staticCache.Isfake = '1';
    //debugger
    if (id) {
        get_purchase_Data(staticCache, $('form'), 'IsTable');
    }
}
function get_purchase_Data(model, $form, state) {
    common.callAjaxWithLoadingSync(_url.get_t_reale_purchase_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            if (state != 'IsTable') {

                BindPenalty(result.data.split('Ʈ')[0]);
            }

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
    if (data.length) {
        let html = "";
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                var seq = data[i]['PenaltySEQ'];
                var psed = ' id=\'psed_' + i + '_' +seq+'\'';
                var ped = ' id=\'ped_' + i + '_' + seq + '\'';

                var pmd = ' id=\'pmd_' + i + '_' + seq + '\'';
                var psn = ' id=\'psn_' + i + '_' + seq + '\'';
                var pbtn = ' id=\'pbtn_' + i + '_' + seq + '\' ';
                var disable = 'disabled';
                if (data[i]['ButtonEnable'] == '1')
                    disable = ' ';
               // var onclick = 
                //<button type="button" class="btn btn-sm btn-primary position-absolute end-1 m-1" ' + pbtn + ' > 編集</button >\

                html +=
                    '<div class="col-12 border-1 m-1 t-reale-list rounded position-relative">\
                    <button type="button" class="btn btn-sm btn-primary position-absolute end-1 m-1" ' + pbtn + ' onclick=rightPanel(this) ' + disable +' > 編集</button >\
               <strong '+ psed + '>' + data[i]['PenaltyStartDate'] + '～' + data[i]['PenaltyEndDate'] + '</strong>\
                    <p'+ pmd + ' style="padding-top:15px">'+ data[i]['PenaltyMemo'] + '</p>\
                    <div class="float-end">\
                        <p>登録スタッフ：<p'+ psn + '>'+ data[i]['TenStaffName'] + '</p>\
                        </p>\
                    </div>\
                                                    </div>'
            }
        }
        $('#penaltyList').html('');
        $('#penaltyList').append(html);
    }
}
function clearBack() {
    $('.t-reale-list').removeClass('t-reale-back-might');
}
function rightPanel(e) {
    clearBack();
    $('#' + e.id).parent().addClass('t-reale-back-might'); 
    document.querySelector("#flexSwitchCheckDefault_Penalty").checked = true;
    document.querySelector("#flexSwitchCheckDefault_Penalty").checked ? $('.disabled-account').removeAttr("disabled") : $('.disabled-account').attr("disabled", "true")
    var id = e.id.split('_')[1] + '_' + e.id.split('_')[2];
    //psed
    $('#seqPenalty').val(e.id.split('_')[2]);
    $('#StartDate').val($('#psed_' + id).text().split('～')[0]);
    $('#EndDate').val($('#psed_' + id).text().split('～')[1]);
    $('#penaltyArea').val($('#pmd_' + id).text());
    SetCache();
}
function BindPenalty(result) {
    let data = JSON.parse(result);
    if (data.length) {
        if (data[0]['TestFlg'] == '1') {
            $('#flexSwitchCheckDefault').attr("checked", "true");
            document.querySelector("#flexSwitchCheckDefault").checked = true;
        }
        else {
            $('#flexSwitchCheckDefault').attr("checked", "false");
            document.querySelector("#flexSwitchCheckDefault").checked = false; 
        }

        //debugger
        if (data[0]['PenaltyFlg'] == '1') {
           // $('#flexSwitchCheckDefault_Penalty').attr("checked", "true");
            //ペナルティのため制限中
            $('#penltyZero').addClass("d-none");
            $('#penltyOne').removeClass("d-none");

        }
        else {
            $('#penltyZero').removeClass("d-none");
            $('#penltyOne').addClass("d-none");
        }
      
        //penltyReason
        $('#penltyReason').text(data[0]['PenaltyMemo']);
        $('#StartDate').val(null);
        $('#EndDate').val(null);
        $('#penaltyArea').val('');
        var peri = data[0]['PenaltyStartDate'] + '～' + data[0]['PenaltyEndDate'];
        if (peri.includes('null'))
            peri = '';
        $('#penaltyPeriod').text( peri); 
    }
}
function l_logfunction() {

    let model = {
        LoginKBN: '3',
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: 't_reale_account',
        Processing: 'Update',
        Remarks: common.getUrlParameter('RealECD'),
    };
    common.callAjax(_url.Insert_L_Log, model, function (result) {
        if (result && result.isOK) {
            return;
        }
    });
}
