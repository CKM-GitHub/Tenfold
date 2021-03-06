const _url = {};
$(function () {
    setValidation(); 
    _url.get_t_reale_purchase_DisplayData = common.appPath + '/t_assess_guide/get_t_assess_guide_DisplayData'; 
    _url.get_Modal_HomeData = common.appPath + '/t_reale_purchase/get_Modal_HomeData';
    _url.get_Modal_ProfileData = common.appPath + '/t_reale_purchase/get_Modal_ProfileData';
    _url.get_Modal_ContactData = common.appPath + '/t_reale_purchase/get_Modal_ContactData';
    _url.get_Modal_DetailData = common.appPath + '/t_reale_purchase/get_Modal_DetailData';
    _url.get_t_assess_guide_url_uploadFiles = common.appPath + '/api/CommonApi/UploadFiles';
    _url.get_t_assess_guide_AttachFiles = common.appPath + '/t_assess_guide/get_t_assess_guide_AttachFiles';
    _url.get_t_assess_guide_DeleteFiles = common.appPath + '/t_assess_guide/DeleteAttachZippedFilePath';
    _url.get_t_assess_guide_DownlaodFiles = common.appPath + '/t_assess_guide/DownloadAttachZippedFilePath';
    _url.get_t_assess_guide_DeliveredView = common.appPath + '/t_assess_guide/ViewedProcess';
    _url.get_t_assess_guide_SaveChanges = common.appPath + '/t_assess_guide/SaveChanges';
    _url.Insert_L_Log = common.appPath + '/t_assess_guide/Insert_L_Log';
    addEvents(); 
});
function setValidation() { 
    $('.form-areamansion')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112
    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
    //.addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111
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
var filesDataTemp = null;
function Addtemp() {
    if (filesDataTemp.length) {

        var html = '';
        $('.count-list-no').remove()
        for (var i = 0; i < filesDataTemp.length; i++) {
           
            var Count = $('.count-list-original').length + $('.count-list-temp').length;
            if (Count > 0) {
                if ((i + Count) > 4)
                    break;
            }
            if (i > 4)
                break;
            
            var size = ''; 
            size ='('+ (parseFloat(filesDataTemp[i].size) / (1024 * 1024)).toFixed(3) + ' MB)'; 
            if ((parseFloat(filesDataTemp[i].size) / (1024 * 1024)).toFixed(3) > $('#inputFile').attr("maxsize")) {
                size += '<i style="color:red; font-size:10px;"> ***' + $('#inputFile').attr("maxsize") +'MB 未満を選択してください。アップロードされません。</i>'
            }
            html += '<div class="card m-1 p-0 bg-light count-list-temp">\
        <div class="card-body mt-1 pt-1 mb-1 pb-1">\
            <div class="d-flex justify-content-between">\
                <div class="d-flex flex-row align-items-center">\
                    <div class="ms-3">\
                        <a href="#" class="text-underline">\
                            <h5>'+ filesDataTemp[i].name+'</h5>\
                        </a>\
                        <p class="small mb-0">未アップロート</p>\
                        <p class="small mb-0"> '+ size + '</p>\
                    </div>\
                </div>\
                <div class="d-flex flex-row align-items-center">\
                    <a href="#" class="text-secondary close-attach" onclick="CloseTemp(this)"><i class="fa fa-close"></i></a>\
                </div>\
            </div>\
        </div>\
    </div>'; 
        }
        var flist = $('#FileList').html();
        $('#FileList').html('');
        $('#FileList').append(html + flist);
    }
}
function CloseTemp(e) {  
    $(e).parent().parent().parent().parent().remove();
    var count = $('.count-list-original').length + $('.count-list-temp').length;
    if (count == 0) {
        addNaShi();
    }

    if ($(e).parent().parent().parent().parent().hasClass('count-list-original')) {
        var cal = $('#list_AttachItem').val();
        $('#list_AttachItem').val(cal + ' ' + $(e).attr('id'));
    }
     
}
function addEvents() {
    $("#Chk_req_Name_Kanji,#Chk_req_pc_Address,#Chk_req_pc_mail_address,#Chk_req_pc_phone,#Chk_CopyInfo").on('change', function (e) {
        Check3Steps(e);
    })
    $("#chk_IsAgreed").on('change', function (e) {
        if ($('#chk_IsAgreed').is(":checked") && $('#hdn_IsAgreedAll').val() == '1') {
            $('#send_customer2').removeAttr("disabled"); 
        }
        else {  
            $('#send_customer2').prop("disabled", "disabled"); 
        }

    })
    $("#return_button").on('click', function () {
        $('#SendCustomer_1').prop("disabled", "disabled");
        $('#hdn_IsSendOK').val('0');
        $('#send_customer2').prop("disabled", "disabled");
        $('#hdn_IsAgreedAll').val('0');

        if ($('#hdn_NeedRefresh').val() == '1')
            $('#btnDisplay').click();
    })
    $("#SendCustomer_1").on('click', function () {
        $('#hdn_IsAgreedAll').val('1');
        $('#chk_IsAgreed').prop('checked', false);
        $('#send_customer2').prop("disabled", "disabled");
    })
    $("#send_customer2").on('click', function () {
        UploadDropFiles(1);
    })
    $("#return_button2").on('click', function () {
        $('#send_customer2').prop(("disabled", "disabled"));
        $('#hdn_IsAgreedAll').val('0');

    })
    $("#return_SendDoneOk2,#RefreshClick").on('click', function () {
        $('#btnDisplay').click();
    })

    $('#inputFile').on('change', function (e) {
        var files = $(this).prop('files')
        filesDataTemp = files;
        //dropFiles(files, $('#fileStatus'));  
        Addtemp();
        //$(this).val(null);
    });
    common.bindValidationEvent('#form1', '');

    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

    $('#StartDate, #EndDate').on('change', function () {
        Date_Compare();

        const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val();
        //if (!common.checkValidityInput($this)) {
        //    return false;
        //}
        let model = {
            StartDate: $start,
            EndDate: $end
        };
        if (model.StartDate && model.EndDate) {
            if (model.StartDate < model.EndDate) {
                $("#StartDate").hideError();
                $("#EndDate").hideError();
                //$("#EndDate").focus();
                return;
            }
            else {
                $("#StartDate").showError(common.getMessage('E111'));
                $("#EndDate").showError(common.getMessage('E111'));
                return;
            }
        }
    });

    $('#btnToday').on('click', function () {
        let today = common.getToday();
        $('#StartDate').val(today);
        $('#EndDate').val(today);
        //Date_Compare();
    });
    $('#btnThisWeek').on('click', function () {
        let start = common.getDayaweekbeforetoday();
        let end = common.getToday();
        $('#StartDate').val(start);
        $('#EndDate').val(end);
        //Date_Compare();

    });
    $('#btnThisMonth').on('click', function () {
        let today = common.getToday();
        let firstDay = common.getFirstDayofMonth();
        $('#StartDate').val(firstDay);
        $('#EndDate').val(today);
        //Date_Compare();

    });
    $('#btnLastMonth').on('click', function () {
        let firstdaypremonth = common.getFirstDayofPreviousMonth();
        let lastdaypremonth = common.getLastDayofPreviousMonth();
        $('#StartDate').val(firstdaypremonth);
        $('#EndDate').val(lastdaypremonth);
        //Date_Compare();

    }); 
    let model = {
        Key: $("#Key").val(),
        Chk_MiShouri: $("#Chk_MiShouri").val(),
        Chk_Shouri: $("#Chk_Shouri").val(),
        Chk_YouKakunin: $("#Chk_YouKakunin").val(),
        Chk_HouShuu: $("#Chk_HouShuu").val(),
        Chk_Soukyaku: $("#Chk_Soukyaku").val(), 
        Range: $("#ddlRange").val(),
        Kanritantou: $("#ddlStaff").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        IsCSV: false,
        LogFlg: 2
    };

    get_purchase_Data(model, this, 'page_load');

    sortTable.getSortingTable("tblPurchaseDetails");
    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#tblPurchaseDetails tbody').empty();
        model = {
            Key: $("#Key").val(),
            Chk_MiShouri: $("#Chk_MiShouri").val(),
            Chk_Shouri: $("#Chk_Shouri").val(),
            Chk_YouKakunin: $("#Chk_YouKakunin").val(),
            Chk_HouShuu: $("#Chk_HouShuu").val(),
            Chk_Soukyaku: $("#Chk_Soukyaku").val(),
            Range: $("#ddlRange").val(),
            Kanritantou: $("#ddlStaff").val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            IsCSV: false,
            LogFlg: 2

        };

        get_purchase_Data(model, $form, 'Display');
    });

    $('#btnCSV').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#tblPurchaseDetails tbody').empty();

       let modelCSV = {
            Key: $("#Key").val(),
            Chk_MiShouri: $("#Chk_MiShouri").val(),
            Chk_Shouri: $("#Chk_Shouri").val(),
            Chk_YouKakunin: $("#Chk_YouKakunin").val(),
            Chk_HouShuu: $("#Chk_HouShuu").val(),
            Chk_Soukyaku: $("#Chk_Soukyaku").val(),
            Range: $("#ddlRange").val(),
            Kanritantou: $("#ddlStaff").val(),
            StartDate: $("#StartDate").val(),
           EndDate: $("#EndDate").val(),
           IsCSV: false,
           LogFlg: 1
        };
        get_purchase_Data(modelCSV, $form, 'csv');
        modelCSV.IsCSV = true;
        modelCSV.LogFlg = 3;
        common.callAjax(_url.get_t_reale_purchase_DisplayData, modelCSV,
            function (result) {
                //sucess
                var table_data = result.data;

                var csv = common.getJSONtoCSV(table_data);
                if (!(csv == "ERROR")) {
                    l_logfunction('000' + ' ' + $('#r_REName').text(), 'csv', '');
                    var downloadLink = document.createElement("a");
                    var blob = new Blob(["\ufeff", csv]);
                    var url = URL.createObjectURL(blob);
                    downloadLink.href = url;
                    let m = new Date();
                    var dateString =
                        m.getUTCFullYear() + "" +
                        ("0" + (m.getUTCMonth() + 1)).slice(-2) + "" +
                        ("0" + m.getUTCDate()).slice(-2) + "_" +
                        ("0" + m.getHours()).slice(-2) + "" +
                        ("0" + m.getMinutes()).slice(-2) + "" +
                        ("0" + m.getSeconds()).slice(-2);
                    downloadLink.download = "処理待ちリスト_" + dateString + ".csv"; 
                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    $('#site-error-modal').modal('show');
                }
            }
        )
    }); 
}

function Check3Steps() {
    if ($('#Chk_req_Name_Kanji').is(":checked") && $('#Chk_req_pc_Address').is(":checked") && $('#Chk_req_pc_mail_address').is(":checked") && $('#Chk_req_pc_phone').is(":checked") && $('#Chk_CopyInfo').is(":checked")) {
        if ($('#hdn_IsSendOK').val() == '1') {
            $('#SendCustomer_1').removeAttr("disabled"); 
        }
        else {  
            $('#SendCustomer_1').prop("disabled", "disabled");
        } 
    }
    else
        $('#SendCustomer_1').prop("disabled", "disabled");



}
function l_logfunction(remark, Process, id) {

    let model = {
        LoginKBN: '3',
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Processing: Process,
        Remarks: remark,
    };
    common.callAjax(_url.Insert_L_Log, model, function (result) {
        if (result && result.isOK) {
            if (remark == 't_seller_assessment ' + id) {
                window.location.href = window.location.href.replace('t_assess_guide', 't_seller_assessment').replace('#','') + "?SellerCD=" + id;
            }
            else if (remark == 't_reale_profile ' + id) {
                window.location.href = window.location.href.replace('t_assess_guide', 't_reale_profile').replace('#', '')  + "?RealECD=" + id;
            }
        }
    });
}
function get_purchase_Data(model, $form, state) {
    //model.IsCSV = false;
    common.callAjaxWithLoading(_url.get_t_reale_purchase_DisplayData, model, this, function (result) {

        if (result && result.isOK) {

            Bind_DisplayData(result.data);
            if (state == 'Display')
                l_logfunction('000' + ' ' + $('#r_REName').text(), 'display', '');
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
function Bind_DisplayData(result) {
    let _letter = "", _class = "", _status = "";
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            var sts = data[i]["ステータス"];
            if (sts == '未処理')
                sts = '<i class="ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal fst-normal">未</i><span\
                    class="font-semibold" > 未処理</span>';
            else if (sts == '処理中')
                sts = '<i class="ms-1 ps-1 pe-1 rounded-circle bg-warning text-white fst-normal fst-normal">処</i><span\
            class="font-semibold" > 処理中</span>';

            else if (sts == '要確認')
                sts = '<i class="ms-1 ps-1 pe-1 rounded-circle bg-danger text-white fst-normal fst-normal">要</i><span\
            class="font-semibold" > 要確認</span>';

            else if (sts == '保留')
                sts = '<i class="ms-1 ps-1 pe-1 rounded-circle bg-dark text-white fst-normal fst-normal">保</i><span\
                    class="font-semibold">保留</span></label>';
            else if (sts == '送客済')
                sts = '<i class="ms-1 ps-1 pe-1 rounded-circle bg-success text-white fst-normal fst-normal">保</i><span\
            class="font-semibold" > 送客済</span>';
            var paramSeller = '\'t_seller_assessment ' + data[i]["SellerCD"] + '\',\'' + 'link' + '\',\'' + data[i]["SellerCD"] + '\'';
            var paramSellerRealeProfile = '\'t_reale_profile ' + data[i]["不動産会社CD"] + '\',\'' + 'link' + '\',\'' + data[i]["不動産会社CD"] + '\'';
            var DeepDatetime = data[i]["DeepDate"].replace(" ", "&");
            var requestInfo = data[i]["MansionName"] + '&' + data[i]["RoomNumber"] + '&' + data[i]["Address"] + '&' + data[i]["買取依頼日時"] + '&' + data[i]["ProgressKBN"] + '&' + data[i]["TenStaffCD"] + '&' + data[i]["IntroPlanDate"] + '&' + data[i]["不動産会社"] + '&' + data[i]["Remark"] + '&' +
                data[i]["NameConfDateTime"] + '&' + data[i]["AddressConfDateTime"] + '&' + data[i]["TelConfDateTime"] + '&' + data[i]["MailConfDateTime"] + '&' + data[i]["RegistConfDateTime"] + '&' + data[i]["IntroReqID"];
            //let requestInfo = {
            //    MansionName: data[i]["MansionName"] 
            //}
            html += '<tr>\
            <td class="text-end">'+ data[i]["No"] + '</td>\
            <td><label class="form-check-label" for="defaultCheck1">\
              '+ sts + ' </td>\
                <td> '+ data[i]["査定ID"] + ' </td>\
                <td> <a class="text-heading font-semibold text-decoration-underline" data-bs-toggle="modal"\
             <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["AssessReqID"] + '&' + DeepDatetime + ' data-bs-toggle="modal" data-bs-target="#mansion" href="#" onclick="Bind_ModalDetails_Show(this.id,\'' + requestInfo + '\')"><span>' + data[i]["物件名"] + '</span></a></td>\
    <td> <a class="text-heading font-semibold text-decoration-underline" onclick="l_logfunction(' + paramSeller + ')" href="#"> ' + data[i]["売主名"] + '\
             </a></td>\
                <td> <a class="text-heading font-semibold text-decoration-underline" onclick="l_logfunction(' + paramSellerRealeProfile + ')" href="#"> ' + data[i]["不動産会社"] + '\
              </a></td>\
                <td> '+ data[i]["管理担当"] + '</td>\
                <td> '+ data[i]["買取依頼日時"] + '</td>\
               <td>'+ data[i]["経過時間"] + '</td>\
                <td> '+ data[i]["送客予定日"] + '</td>\
            </tr>'
        }
        $('#total_record').text("検索結果： " + data.length + "件");
        $('#total_record_up').text("検索結果： " + data.length + "件");
        $('#no_record').text("");
    } else {
        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件 ");
        $('#no_record').text("表示可能データがありません");
    }
    $('#tblPurchaseDetails tbody').append(html);
    $('.nav-link').removeClass("active");
    $('#anchor_guide').addClass("active");

}
function Bind_ModalDetails_Show(id, request) { 
    $('#pills-info-tab').addClass("active");
    $('#hdn_NeedRefresh').val(request.split('&')[4]);
    //Insert View hdn_NeedRefresh
    let model = {
        Key: $("#Key").val(),
        Chk_MiShouri: $("#Chk_MiShouri").val(),
        Chk_Shouri: $("#Chk_Shouri").val(),
        Chk_YouKakunin: $("#Chk_YouKakunin").val(),
        Chk_HouShuu: $("#Chk_HouShuu").val(),
        Chk_Soukyaku: $("#Chk_Soukyaku").val(),
        Range: $("#ddlRange").val(),
        Kanritantou: $("#ddlStaff").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        IntroReqID: request.split('&')[14],
        ProcessKBN: request.split('&')[4],
        IsCSV: false,
        LogFlg: 1
    };
    common.callAjax(_url.get_t_assess_guide_DeliveredView, model, function (result) {
        if (result && result.isOK) {
            let data = JSON.parse(result.data);
            if (data.length) {
                var i = 0;
                var id = data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["AssessReqID"] + '&' + data[i]["DeepDate"].replace(" ", "&")
                var requestInfo = data[i]["MansionName"] + '&' + data[i]["RoomNumber"] + '&' + data[i]["Address"] + '&' + data[i]["買取依頼日時"] + '&' + data[i]["ProgressKBN"] + '&' + data[i]["TenStaffCD"] + '&' + data[i]["IntroPlanDate"] + '&' + data[i]["不動産会社"] + '&' + data[i]["Remark"] + '&' +
                    data[i]["NameConfDateTime"] + '&' + data[i]["AddressConfDateTime"] + '&' + data[i]["TelConfDateTime"] + '&' + data[i]["MailConfDateTime"] + '&' + data[i]["RegistConfDateTime"] + '&' + data[i]["IntroReqID"] + '&' + data[i]["SendOK"];
                Bind_ModalDetails(id, requestInfo);
            }
        }
    })
}
function dropFiles(files, obj) {
    if (files != null && files != 'undefined') {
        formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            fileCount++;
            var key = 'file' + fileCount.toString() + '_' + files[i].size + '_' + i;
            if ((parseFloat(filesDataTemp[i].size) / (1024 * 1024)).toFixed(3) < $('#inputFile').attr("maxsize"))
                formData.append(key, files[i]);
        }
        var status = new createFileStatusbar();
        obj.after(status.statusbar);
        sendFileToServer(formData, status);
    }
    deleteFileFromServer();
}
function UploadDropFiles(IsBypass) { 
    dropFiles(filesDataTemp, $('#fileStatus')); 
    $('#inputFile').val(null);
    t_Assess_Guide_SaveChanges(IsBypass);
}
let model_Cache = {
    IntroReqID: null,
    NameConf: 0,
    AddressConf: 0,
    TelConf: 0,
    MailConf: 0,
    RegisConf:0,
    TenStaffCD: null,
    ProcessKBN: 0,
    IntroPlanDate: null,
    Remark: null,
    IsSomethingChanged:0
}
function t_Assess_Guide_SaveChanges(IsBypass)
{
    let c_model_Cache = {
        IntroReqID: $('#hdn_AssReID').val(),
        NameConf: $('#Chk_req_Name_Kanji').is(":checked")? 1:0,
        AddressConf: $('#Chk_req_pc_Address').is(":checked") ? 1 : 0,
        TelConf: $('#Chk_req_pc_phone').is(":checked") ? 1 : 0,
        MailConf: $('#Chk_req_pc_mail_address').is(":checked") ? 1 : 0,
        RegisConf: $('#Chk_CopyInfo').is(":checked") ? 1 : 0,
        TenStaffCD: $('#req_ddlStaff').val(),
        ProcessKBN: $('#req_ddlstatus').val(),
        IntroPlanDate: $('#req_SendDate').val(),
        Remark: $('#req_remarks').val(),
        IsSomethingChanged: 0
    }
    let model = {
        IntroReqID: c_model_Cache.IntroReqID,
        NameConf: model_Cache.NameConf == c_model_Cache.NameConf ? 0 : 1,
        AddressConf: model_Cache.AddressConf == c_model_Cache.AddressConf ? 0 : 1,
        TelConf: model_Cache.TelConf == c_model_Cache.TelConf ? 0 : 1,
        MailConf: model_Cache.MailConf == c_model_Cache.MailConf ? 0 : 1,
        RegisConf: model_Cache.RegisConf == c_model_Cache.RegisConf ? 0 : 1,
        TenStaffCD: c_model_Cache.TenStaffCD,
        ProcessKBN: c_model_Cache.ProcessKBN,
        IntroPlanDate: c_model_Cache.IntroPlanDate,
        Remark: c_model_Cache.Remark,
        IsSomethingChanged: 1,
        IsByPass: (IsBypass ==1 )? 1 : 0
    }
    common.callAjax(_url.get_t_assess_guide_SaveChanges, model, function (result) {
        if (result && result.isOK) {
            if (IsBypass == 1)
                $('#modal-sendok').modal('show');
            else
            $('#modal-changed').modal('show')
        }
        else
            $('#site-error-modal').modal('show');
    });
}
function createFileStatusbar() {
    this.statusbar = $('<div class="statusbar"></div>');
    this.progressBar = $('<div class="progressBar"><div></div></div>').appendTo(this.statusbar);

    this.setProgress = function (percent) {
        var progressBarWidth = percent * this.progressBar.width() / 100;
        this.progressBar.find('div').animate({ width: progressBarWidth }, 10).html(percent + "% ");
    }
}
var formData = new FormData();
var fileCount = 0;
var url_uploadFiles = _url.get_t_assess_guide_url_uploadFiles; 
function deleteFileFromServer() {
    let modeldelete = {
        AttachSEQ: $('#list_AttachItem').val()
    }
    common.callAjax(_url.get_t_assess_guide_DeleteFiles, modeldelete, function (result) {
        if (result && result.isOK) {
            // 
        }
        
    })
} 
function downloadAttach(e) {
    let modeldown = {
        AttachSEQ: $(e).attr('id'),
        FileName: $(e).text()
    } 
    $.ajax({
        url: _url.get_t_assess_guide_DownlaodFiles,
        type: "POST",
        contentType: 'application/json; charset=utf-8', 
        data: JSON.stringify(modeldown),
        headers: {
            RequestVerificationToken: $("#_RequestVerificationToken").val(),
        },
        async: true,
        timeout: 100000,
    }).done(function (data) { 
        var downloadLink = document.createElement("a"); 
        downloadLink.href = _url.get_t_assess_guide_DownlaodFiles;
        downloadLink.download = modeldown.FileName.trim(); 
        document.body.appendChild(downloadLink);
        downloadLink.click();
        document.body.removeChild(downloadLink);
        
    }).fail(function (XMLHttpRequest, status, e) { 

    });
}
function sendFileToServer(fileData, status) {
    fileData.delete('UserCD'); 
    fileData.delete('IntroReqID');

    fileData.append('UserCD', $('#UserCD').val()); 
    fileData.append('IntroReqID', $('#hdn_AssReID').val()); 
    calltoApiController_FileUploadHandle(common.appPath + '/api/CommonApi/UploadFiles', fileData,
        function (result) {  
            clearFileInfo(); 
            if (!result) return;
            if (result.MessageID) { 
                return;
            }  
        },
        function () { 
            clearFileInfo();
        },
        function (percent) {  
            status.setProgress(percent);
        });
}

function calltoApiController_FileUploadHandle(url, fileData, callback, callbackerror, callbackprogress) {
   
    var obj = {
        url: url,
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: fileData,
        timeout: 600000,
        headers: {
            RequestVerificationToken: getApiAuthorization(),
        },
        success: function (data) {
         
            if (callback) callback(JSON.parse(data));
        },
        error: function (err) {
           
            alert(err.status + ":" + err.statusText);
            if (callbackerror) callbackerror();
        }
        
    };
    var jqXHR = $.ajax(obj);
}
 
function clearFileInfo() {
    $('#fileStatus').nextAll().remove();
    formData = new FormData();
    fileCount = 0;
}
function getApiAuthorization() {
    return $("#_RequestVerificationToken").val()
}
//


 

