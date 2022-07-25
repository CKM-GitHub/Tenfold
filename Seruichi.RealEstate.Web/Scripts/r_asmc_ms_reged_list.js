const _url = {};
let Radio_Checkbox = 0;
let Exp_Checkbox = 0;
$(function () {
    setValidation();
    _url.Get_DataList = common.appPath + '/r_asmc_ms_reged_list/Get_DataList';
    _url.InsertL_Log = common.appPath + '/r_asmc_ms_reged_list/Insert_l_log';   
    addEvents();
    //$('#MansionName').focus();
});
function setValidation() {
    //$('#MansionName')
    //    .addvalidation_errorElement("#errorMansionName")
    //    .addvalidation_maxlengthCheck(25)//E105
    //    .addvalidation_doublebyte(); 


    $('#StartYear')
        .addvalidation_errorElement("#errorYear")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104
        .addvalidation_datecompare(); //E113

    $('#EndYear')
        .addvalidation_errorElement("#errorYear")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number() //E104
        .addvalidation_datecompare(); //E113

    $('#btnDisplay')
        .addvalidation_errorElement("#errorbtnDisplay"); //E303

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
            if (Number(model.StartDate) <= Number(model.EndDate)) {
                $("#StartYear").hideError();
                $("#EndYear").hideError();
                $("#EndYear").focus();
                return;
            }
        }
    });

    $('#RadioCheck').on('change', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        this.value = this.checked ? 1 : 0;
        Radio_Checkbox = this.value;
    });

    $('#defaultCheck3').on('change', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        this.value = this.checked ? 1 : 0;
        Exp_Checkbox = this.value;
    });


    sortTable.getSortingTable("r_table_List");
    if ($('#defaultCheck3').val() == 1)
    {
        Exp_Checkbox = $('#defaultCheck3').val();
        AddData();
    }
   
    $('#btnDisplay').on('click', function () {
        AddData();
    });

    ////////$(document).on('click', '.tree li  input[type="checkbox"]', function () {
    ////////    $(this).closest('li').find('ul input[type="checkbox"]').prop('checked', $(this).is(':checked'));
    ////////}).on('click', '.node-item', function () {
    ////////    var parentNode = $(this).parents('.tree ul');
    ////////    if ($(this).is(':checked')) {

    ////////        var elementschk = parentNode.find('ul input[type="checkbox"]:checked');
    ////////        var elements = parentNode.find('ul input[type="checkbox"]');
    ////////        if (elements.length == elementschk.length) {
    ////////            parentNode.find('li .node-parent').prop('checked', true);
    ////////        }
    ////////        else {
    ////////            parentNode.find('li .node-parent').prop('checked', false);
    ////////        }
    ////////    } else {
    ////////        var elementschk = parentNode.find('ul input[type="checkbox"]:checked');
    ////////        var elements = parentNode.find('ul input[type="checkbox"]');
    ////////        if (elements.length == elementschk.length) {
    ////////            parentNode.find('li .node-parent').prop('checked', true);
    ////////        }
    ////////        else {
    ////////            parentNode.find('li .node-parent').prop('checked', false);
    ////////        }
    ////////    }
    ////////});
}

function AddData()
{
    $form = $('#form1').hideChildErrors();

    if (!common.checkValidityOnSave('#form1')) {
        $form.getInvalidItems().get(0).focus();
        return false;
    }
    $('#r_table_List tbody').empty();

    const $MansionName = $("#MansionName").val().trim(), $StartYear = $("#StartYear").val(), $EndYear = $('#EndYear').val()

    var cityGPCD_check = '';
    var gp_length = 0;
    $('.node-parent:checkbox:checked').each(function () {
        cityGPCD_check += $(this).val() + ',';
        gp_length += 1;
    });

    var cityCD_check = '';
    var city_lenght = 0;
    $('.node-item:checkbox:checked').each(function () {
        cityCD_check += $(this).val() + ',';
        city_lenght += 1;
    });

    var Rdo_Rating = '';
    if (Radio_Checkbox == 1) {
        $('.form-check-input:radio:checked').each(function () {
            Rdo_Rating = $(this).val();
        });
    }

    var Check_Expired = '';
    if (Exp_Checkbox == 1) {
        $('.form-check-input:checkbox:checked').each(function () {
            Check_Expired = $(this).val();
        });
    }

    let model = {
        MansionName: $MansionName,
        StartYear: Get_FT_Age($EndYear, 'F'),
        EndYear: Get_FT_Age($StartYear, 'T'),
        CityCD: cityCD_check.slice(0, -1),
        CityGPCD: cityGPCD_check.slice(0, -1),
        Radio_Rating: Rdo_Rating,
        Check_Expired: Check_Expired
    };

    if (model.MansionName == "" && model.StartYear == "" && model.EndYear == "" && model.CityCD == "" && model.CityGPCD == "" && model.Radio_Rating == "" && model.Check_Expired =="") {
        $('#btnDisplay').showError(common.getMessage('E303'));
        $('#MansionName').focus();
        $('#total_record').text("");
        $('#total_record_up').text("");
        $('#no_record').text("");
    }
    else {
        $('#btnDisplay').hideError();
        Get_DataList(model, $form);
    }


}

function Get_FT_Age(age, type) {

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    var start_yyyymm = '', end_yyyymm = '';

    //Forｍ.築年数（To） => 築年月(From)
    if (type == 'F') {
        if (age !== '') {
            start_yyyymm = (yyyy - age) + String(parseInt(mm) + 1).padStart(2, '0');
        }
        return start_yyyymm;
    }
    //Forｍ.築年数（From） => 築年月(To)
    else if (type == 'T') {
        if (age !== '') {
            end_yyyymm = (yyyy - (age - 1)) + mm;
        }
        return end_yyyymm;
    }
}

function Get_DataList(model, $form) {
    common.callAjaxWithLoading(_url.Get_DataList, model, this, function (result) {
        
        if (result && result.isOK) {

            Bind_tbody(result.data);
        }
        if (result && !result.isOK) {
            if (result.message.MessageText1 != "") {
                $('#btnDisplay').showError(result.message.MessageText1);
            }
            else {
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                    $form.getInvalidItems().get(0).focus();
                }
            }
        }
    });
}


function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (data[i]["公開フラグ"] == "未公開") {
                _class = "text-danger";
            }
            else
            {
                _class = "cap-text-primary";
            }

            html += '<tr>\
            <td class= "text-end">' + data[i]["NO"] + '</td>\
            <td> <a class="text-heading font-semibold text-decoration-underline" href="#"  onclick="l_logfunction(this.id)" id='+ data[i]["マンションCD"] + '&' + data[i]["マンション名"]+'>'+ data[i]["マンション名"] + '</a> </td>\
            <td> <a class="text-heading font-semibold">'+ data[i]["住所"] + '</a> </td>\
            <td> <span class='+ _class + '>(' + data[i]["公開フラグ"] + ')</span></td>\
            <td> '+ data[i]["登録日"] + '</td>\
            <td> '+ data[i]["有効期限日"] + '</td>\
            <td>\
            <div class="d-flex flex-row mt-2">\
            <div class="text-danger mb-1 me-2">'
                + data[i]["優先度マーク"] +  '\
           </div>\
           </div>\
           </td>\
            <td class="d-none">'+ data[i]["マンションCD"] + '</td>\
            <td class="d-none"> '+ data[i]["住所カナ"] + '</td>\
            <td class="d-none"> '+ data[i]["Priority"] + '</td>\
          </tr>'
        }
        $('#total_record').text("検索結果：" + data.length + "件")
        $('#total_record_up').text("検索結果：" + data.length + "件")
        $('#no_record').text("");
    }
    else {
        $('#total_record').text("検索結果： 0件")
        $('#total_record_up').text("検索結果： 0件")
        $('#no_record').text("表示可能データがありません");
    }
    $('#r_table_List tbody').append(html);

   
}


function l_logfunction(id) {
    
    let model = {
        LogDateTime: null,
        LoginKBN: null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        PageID: "r_asmc_ms_reged_list",
        Processing: null,
        Remarks: null,
        MansionCD: id.split('&')[0],
        MansionName: id.split('&')[1],
    };
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
                if (model.PageID == "r_asmc_ms_reged_list")
                {
                    window.location.href = common.appPath + '/r_asmc_set_ms/Index?mc=' + model.MansionCD;
                    //alert("https://www.seruichi.com/r_asmc_set_ms/Index?MansionCD=" + model.MansionCD)
                }
            }
            if (result && !result.isOK) {

            }
        });
}

