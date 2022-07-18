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
    
    addEvents(); 
});
function setValidation() { 

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
    //$('#subMenu li').children('a').on('click', function () {
    //    $('#subMenu li').children('a').removeClass("active");
    //    $(this).addClass('active');
    //});
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
        IsCSV: false

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
            IsCSV: false
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

        model = {
            RealECD: common.getUrlParameter('RealECD'),
            Chk_Area: $("#Chk_Area").val(),
            Chk_Mansion: $("#Chk_Mansion").val(),
            Chk_SendCustomer: $("#Chk_SendCustomer").val(),
            Chk_Top5: $("#Chk_Top5").val(),
            Chk_Top5Out: $("#Chk_Top5Out").val(),
            Chk_NonMemberSeller: $("#Chk_NonMemberSeller").val(),
            Range: $("#ddlRange").val(),
            StartDate: $("#StartDate").val(),
            EndDate: $("#EndDate").val(),
            IsCSV: true
        };
        get_purchase_Data(model, $form, 'csv');
        common.callAjax(_url.get_t_reale_purchase_CSVData, model,
            function (result) {
                //sucess
                var table_data = result.data;

                var csv = common.getJSONtoCSV(table_data);
                if (!(csv == "ERROR")) {
                    l_logfunction(model.RealECD + ' ' + $('#r_REName').text(), 'csv', '');
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
                    downloadLink.download = "不動産会社査定状況リスト_" + dateString + ".csv";
                    //"不動産会社査定状況リスト_YYYYMMDD_HHMMSS.csv"
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

function get_purchase_Data(model, $form, state) {
    model.IsCSV = false;
    common.callAjaxWithLoading(_url.get_t_reale_purchase_DisplayData, model, this, function (result) {

        if (result && result.isOK) {

            Bind_DisplayData(result.data);
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

            var DeepDatetime = data[i]["DeepDate"].replace(" ", "&");
            var requestInfo = data[i]["MansionName"] + '&' + data[i]["RoomNumber"] + '&' + data[i]["Address"] + '&' + data[i]["買取依頼日時"] + '&' + data[i]["ProgressKBN"] + '&' + data[i]["TenStaffCD"] + '&' + data[i]["IntroPlanDate"] + '&' + data[i]["不動産会社"] + '&' + data[i]["Remark"] + '&'+
                data[i]["NameConfDateTime"] + '&' + data[i]["AddressConfDateTime"] + '&' + data[i]["TelConfDateTime"] + '&' + data[i]["MailConfDateTime"] + '&' + data[i]["RegistConfDateTime"] + '&' + data[i]["IntroReqID"]  ;
            //let requestInfo = {
            //    MansionName: data[i]["MansionName"] 
            //}
            html += '<tr>\
            <td class="text-end">'+ data[i]["No"] + '</td>\
            <td><label class="form-check-label" for="defaultCheck1">\
              '+ sts +' </td>\
                <td> '+ data[i]["査定ID"] + ' </td>\
                <td> <a class="text-heading font-semibold text-decoration-underline" data-bs-toggle="modal"\
             <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["AssessReqID"] + '&' + DeepDatetime + ' data-bs-toggle="modal" data-bs-target="#mansion" href="#" onclick="Bind_ModalDetails(this.id,\'' + requestInfo + '\')"><span>' + data[i]["物件名"] + '</span></a></td>\
    <td> <a class="text-heading font-semibold text-decoration-underline" href="#"> '+ data[i]["売主名"]+'\
             </a></td>\
                <td> <a class="text-heading font-semibold text-decoration-underline" href="#"> '+ data[i]["不動産会社"] +'\
              </a></td>\
                <td> '+ data[i]["管理担当"] +'</td>\
                <td> '+ data[i]["買取依頼日時"] +'</td>\
               <td>'+ data[i]["経過時間"] +'</td>\
                <td> '+ data[i]["送客予定日"] +'</td>\
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
function UploadDropFiles() { 
    dropFiles(filesDataTemp, $('#fileStatus')); 
    $('#inputFile').val(null);
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
var url_uploadFiles = _url.get_t_assess_guide_url_uploadFiles;//'@Url.Action("UploadFiles", "api/InputBukkenShousaiApi")'; //_url.get_t_assess_guide_url_uploadFiles;// 
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
        AttachSEQ: $(e).attr('id')
    }
    common.callAjax(_url.get_t_assess_guide_DownlaodFiles, modeldown, function (result) {
        if (result && result.isOK) {
            alert(result.data)
            // 
        }
        else {
            alert(result.data)
        }

    })
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
        //, oncomplete: function () {
        //    $('.statusbar').hide();
        //}
        
    };
    //if (callbackprogress) {
    //    obj.xhr = function () {
    //        var xhrobj = $.ajaxSettings.xhr();
    //        if (xhrobj.upload) {
    //            xhrobj.upload.addEventListener('progress', function (event) {
    //                var percent = 0;
    //                var position = event.loaded || event.position;
    //                var total = event.total;
    //                if (event.lengthComputable) {
    //                    percent = parseInt(position / total * 10000) / 100;
    //                }
    //                callbackprogress(percent);
    //            }, false);
    //        }
    //        return xhrobj;
    //    }
    //}
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


 

