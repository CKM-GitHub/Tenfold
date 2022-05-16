const _url = {};
$(function () {
    setValidation();
    _url.Get_DataList = common.appPath + '/r_asmc_ms_reged_list/Get_DataList';
    addEvents();
});
function setValidation() {
    $('#MansionName')
        .addvalidation_errorElement("#errorMansionName")
        .addvalidation_maxlengthCheck(25)//E105
        .addvalidation_doublebyte(); 

    $('#StartYear')
        .addvalidation_errorElement("#errorStartYear")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104
        .addvalidation_datecompare(); //E113

    $('#EndYear')
        .addvalidation_errorElement("#errorEndYear")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number() //E104
        .addvalidation_datecompare(); //E113
}
function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#StartYear, #EndYear').on('change', function () {

        const $this = $(this), $start = $('#StartYear').val(), $end = $('#EndYear').val()

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            StartDate: $start,
            EndDate: $end
        };

        if (model.StartDate && model.EndDate) {
            if (model.StartDate < model.EndDate) {
                $("#StartYear").hideError();
                $("#EndYear").hideError();
                $("#EndYear").focus();
                return;
            }
        }
    });

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#r_table_List tbody').empty();

        const $Apartment = $("#MansionName").val(), $StartYear = $("#StartYear").val(), $EndYear = $('#EndYear').val(),
            $StartUnit = $("#StartUnit").val(), $EndUnit = $("#EndUnit").val()

        let model = {
            Apartment: $Apartment,
            StartAge: Get_FT_Age($EndAge, 'F'),
            EndAge: Get_FT_Age($StartAge, 'T'),
            StartDate: $StartUnit,
            EndDate: $EndUnit,
        };
        Get_DataList(model, $form);

    });
}


function Get_DataList(model, $form) {
    common.callAjaxWithLoading(_url.Get_DataList, model, this, function (result) {
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
    if (data.length > 0) {
        html += '<tr>\
            <td class= "text-end" > ' + (i + 1) + '</td>\
            <td>' + data[i]["マンションCD"] + '</td >\
            <td> <a href="#"  onclick="l_logfunction(this.id)" class="text-heading font-semibold text-decoration-underline" id='+ data[i]["マンションCD"] + '>' + data[i]["マンション名"] + '</a></td>\
            <td>' + data[i]["住所"] + ' </td>\
            <td>' + data[i]["築年月"] + '</td>\
            <td>' + data[i]["築年数"] + '</td>\
            <td>' + data[i]["総戸数"] + '</td>\
            </tr>'

        $('#total_record').text("検索結果：" + data.length + "件")
        $('#total_record_up').text("検索結果：" + data.length + "件")
    }
    else {
        $('#total_record').text("検索結果： 0件")
        $('#total_record_up').text("検索結果： 0件")
    }
    $('#r_table_List tbody').append(html);

    sortTable.getSortingTable("r_table_List");
}