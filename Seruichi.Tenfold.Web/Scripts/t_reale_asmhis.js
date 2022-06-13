const _url = {};
$(function () {
    setValidation();
    _url.get_t_reale_CompanyInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyInfo';
    _url.get_t_reale_CompanyCountingInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyCountingInfo';
    _url.get_t_reale_purchase_DisplayData = common.appPath + '/t_reale_asmhis/get_t_reale_asmhis_DisplayData';

    _url.get_t_reale_purchase_CSVData = common.appPath + '/t_reale_asmhis/get_t_reale_asmhis_DisplayData';
    _url.Insert_L_Log = common.appPath + '/t_reale_purchase/Insert_L_Log';
    _url.get_Modal_HomeData = common.appPath + '/t_reale_purchase/get_Modal_HomeData';
    _url.get_Modal_ProfileData = common.appPath + '/t_reale_purchase/get_Modal_ProfileData';
    _url.get_Modal_ContactData = common.appPath + '/t_reale_purchase/get_Modal_ContactData';
    _url.get_Modal_DetailData = common.appPath + '/t_reale_purchase/get_Modal_DetailData';
    addEvents(); 
    $('#subMenu li').children('a').removeClass("active"); 
    $('#subMenu li').children('a').eq(1).addClass('active');
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_asmhis').addClass('font-bold text-underline');  
});

function setValidation() {
    $('.form-areamansion')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111
}

function addEvents() {
    common.bindValidationEvent('#form1', '');
 
    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

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

    $('#btnToday').on('click', function () {
        let today = common.getToday();
        $('#StartDate').val(today);
        $('#EndDate').val(today);
    });
    $('#btnThisWeek').on('click', function () {
        let start = common.getDayaweekbeforetoday();
        let end = common.getToday();
        $('#StartDate').val(start);
        $('#EndDate').val(end);
    });
    $('#btnThisMonth').on('click', function () {
        let today = common.getToday();
        let firstDay = common.getFirstDayofMonth();
        $('#StartDate').val(firstDay);
        $('#EndDate').val(today);
    });
    $('#btnLastMonth').on('click', function () {
        let firstdaypremonth = common.getFirstDayofPreviousMonth();
        let lastdaypremonth = common.getLastDayofPreviousMonth();
        $('#StartDate').val(firstdaypremonth);
        $('#EndDate').val(lastdaypremonth);
    });
    $('#subMenu li').children('a').on('click', function () {
        $('#subMenu li').children('a').removeClass("active");
        $(this).addClass('active');
    });
    let model = {
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
        IsCSV: false

    };

    $('#seller').addClass('d-none');
    //$('#submenu_seller').addClass('d-none');

    Bind_Company_Data(this);         //Bind Company Info Data to the title part of the page

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
            if (state == 'Display')
                l_logfunction(model.RealECD + ' ' + $('#r_REName').text(), 'display', '');
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
    let _letter = "", _class = "", _status = "";
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            var spantxt = '';
            if (data[i]["AssessType"] == "エリア") {
                spantxt += '<label> <span class="ms-1 ps-1 pe-1 rounded-circle bg-primary text-white">エ</span> エリア</label>';
            }
            else {
                spantxt += '<label> <span  class="ms-1 ps-1 pe-1 rounded-circle bg-warning txt-dark" > マ</span > マンション</label> '; 
            }
            if (data[i]["SendCustomer"] == '送客') {
                spantxt+= '<label> <span class="ms-1 ps-1 pe-1 rounded-circle bg-danger text-white">送</span> 送客</label>';
            }
            //debugger;
            //var DeepDatetime = '';
        //    if (data[i]["DeepDate"])
            //DeepDatetime = '2020'//data[i]["DeepDate"].replace(" ", "&");
            var DeepDatetime = data[i]["DeepDate"].replace(" ", "&");
            var paramSeller = '\'t_seller_assessment ' + data[i]["SellerCD"] + '\',\'' + 'link' + '\',\'' + data[i]["SellerCD"] + '\'';
            var paramDetail = '\'t_seller_assessment_detail ' + data[i]["AssessReqID"] + '\',\'' + 'link' + '\',\'' + data[i]["AssessReqID"] + '\'';

            html += '<tr>\
            <td class="text-nowrap">'+ spantxt + '</td>\
            <td class="text-nowrap">' + data[i]["AssessReqID"] + '</td>\
             <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["AssessReqID"] + '&' + DeepDatetime + ' data-bs-toggle="modal" data-bs-target="#t_reale" href="#" onclick="Bind_ModalDetails(this.id)"><span>' + data[i]["RoomNumber"] + '</span></a></td>\
            <td> <a class="text-heading font-semibold text-decoration-underline text-nowrap" id="' + data[i][" SellerCD"] + '"   onclick="l_logfunction(' + paramSeller +')" > ' + data[i]["SellerName"] + '</a ></td>\
            <td class="text-nowrap">'+ data[i]["InsertDate"] + '</td>\
            <td class="text-nowrap">'+ data[i]["EasyDate"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id="' + data[i]["AssessReqID"] + '"  onclick="l_logfunction(' + paramDetail + ')">' + data[i]["DeepDate"] + '</a></td>\
            <td class="text-nowrap">'+ data[i]["PurchaseDate"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["IntroDate"] + '</td>\
            <td class="text-nowrap" style="text-align:right;"> '+ data[i]["Rank"] + '</td>\
            </tr>'
        }

        $('#total_record').text("検索結果： " + data.length + "件");
        $('#total_record_up').text("検索結果： " + data.length + "件");
        $('#no_record').text("");
    }
    else {
        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件 ");
        $('#no_record').text("表示可能データがありません");
    }
    $('#tblPurchaseDetails tbody').append(html);
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
                window.location.href = window.location.href.replace('t_reale_asmhis', 't_seller_assessment')  + "&SellerCD=" + id;
            }
            else if (remark == 't_seller_assessment_detail ' + id) { 
                window.location.href = window.location.href.replace('t_reale_asmhis', 't_seller_assessment_detail')  + "&AssReqID=" + id;
            }
        }
    });
}
