const _url = {};
$(function () {
     setValidation();
    _url.getM_SellerMansionList = common.appPath + '/t_seller_mansion/GetM_SellerMansionList';
    _url.generate_M_SellerMansionCSV = common.appPath + '/t_seller_mansion/Generate_M_SellerMansionCSV';
    _url.insert_l_log = common.appPath + '/t_seller_mansion/Insert_l_log';
    addEvents();
});
function setValidation() {
    $('#MansionName')
        .addvalidation_errorElement("#errorMansionName")
        .addvalidation_doublebyte(); //E105,E107

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112

}
function addEvents()
{
    common.bindValidationEvent('#form1', '');

    const $Chk_Mi = $("#Chk_Mi").val(), $Chk_Kan = $("#Chk_Kan").val(), $Chk_Satei = $("#Chk_Satei").val(),
        $Chk_Kaitori = $("#Chk_Kaitori").val(), $Chk_Kakunin = $("#Chk_Kakunin").val(), $Chk_Kosho = $("#Chk_Kosho").val(),
        $Chk_Seiyaku = $("#Chk_Seiyaku").val(), $Chk_Urinushi = $("#Chk_Urinushi").val(), $Chk_Kainushi = $("#Chk_Kainushi").val(),
        $MansionName = $("#MansionName").val(), $Range = $("#Range").val(), $StartDate = $("#StartDate").val(),
        $EndDate = $("#EndDate").val()

    let model = {
        Chk_Mi: $Chk_Mi,
        Chk_Kan: $Chk_Kan,
        Chk_Satei: $Chk_Satei,
        Chk_Kaitori: $Chk_Kaitori,
        Chk_Kakunin: $Chk_Kakunin,
        Chk_Kosho: $Chk_Kosho,
        Chk_Seiyaku: $Chk_Seiyaku,
        Chk_Urinushi: $Chk_Urinushi,
        Chk_Kainushi: $Chk_Kainushi,
        MansionName: $MansionName,
        Range: $Range,
        StartDate: $StartDate,
        EndDate: $EndDate,
    };
    getM_SellerMansionList(model,this);
   
    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

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

    $('#btnProcess').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        getM_SellerMansionList(model, $form);
    });
    $('#btnCSV').on('click', function () {
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);

        common.callAjaxWithLoading(_url.generate_M_SellerMansionCSV, model,
            function (result) {
                if (result && result.isOK) {
                    alert("Export Successfully!")
                }
            })

    });
    $('#mansiontable').find('td').click(function () {
        alert($(this).text());
    })
}


function getM_SellerMansionList(model, $form) {
    common.callAjaxWithLoading(_url.getM_SellerMansionList, model, this, function (result) {
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

function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<tr>\
            <td class= "text-end" > ' + data[i]["NO"] + '</td>\
            <td><i class="ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal fst-normal">未</i><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td>\
            <td> ' + data[i]["物件CD"] + ' </td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["MansionCD"] +'&t_mansion_detail'+ '  href="#" onclick="l_logfunction(this.id)"> 物件名物件名物件名物件名物件名</a><p> <small class="text-wrap w-100">'+ data[i]["住所"] + '</small></p></td>\
            <td> '+ data[i]["部屋"] + '</td>\
            <td class="text-end">'+ data[i]["階数"] + '</td>\
            <td class="text-end">'+ data[i]["面積"] + '</td>\
            <th class="text-decoration-underline text-nowrap">'+ data[i]["売主名"] + '</th>\
            <td class="text-nowrap"> '+ data[i]["登録日時"] + '</td>\
            <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#"> '+ data[i]["詳細査定日時"] + ' </a> </td>\
            <td class="text-nowrap"> '+ data[i]["買取依頼日時"] + ' </td>\
            <td class="text-nowrap">\
            <a class="text-heading font-semibold text-decoration-underline" href="#"> 不動産会社名不動産会社名 </a>\
            <p class="text-end">'+ data[i]["マンション金額"] + '</p>\
             </td>\
             <td class="text-nowrap">\
             <a class="text-heading font-semibold text-decoration-underline" href="#"> 不動産会社名不動産会社名 </a>\
             <p class="text-end">'+ data[i]["エリア金額"] + '</p>\
             </td>\
             <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#"> 不動産会社名不動産会社名 </a><p class="text-end">'+ data[i]["買取依頼金額"] + '</p> </td>\
             <td class="text-nowrap"> '+ data[i]["送客日時"] + ' </td>\
             <td class="text-nowrap"> '+ data[i]["成約日時"] + ' </td>\
             <td class="text-nowrap"> '+ data[i]["辞退日時"] + '</td>\
            </tr>'
    }
    $('#mansiontable tbody').append(html);
}



function l_logfunction(id) {   
    let model = {
        LoginKBN:null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: null,
        Processing: null,
        Remarks: null,
        LogId: id.split('&')[0],
        LogStatus: id.split('&')[1]
        
    };
    common.callAjax(_url.insert_l_log, model,
        function (result) {
            if (result && result.isOK) {
                
            }
            if (result && !result.isOK) {
                
            }
        });
}