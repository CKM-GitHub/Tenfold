const _url = {};

$(function () {
    setValidation();
    _url.getM_MansionList = common.appPath + '/t_mansion_list/GetM_MansionList';
    _url.generate_CSV1 = common.appPath + '/t_mansion_list/Generate_CSV1';
    _url.generate_CSV2 = common.appPath + '/t_mansion_list/Generate_CSV2';
    _url.generate_CSV3 = common.appPath + '/t_mansion_list/Generate_CSV3';
    _url.InsertL_Log = common.appPath + '/t_mansion_list/InsertM_Mansion_L_Log';
    addEvents();
});

function setValidation() {
    
    $('#StartNum')
        .addvalidation_errorElement("#errorAge")
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(2)
        .addvalidation_numcompare();

    $('#EndNum')
        .addvalidation_errorElement("#errorAge")
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(2)
        .addvalidation_numcompare();

    $('#StartUnit')
        .addvalidation_errorElement("#errorUnit")
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(2);


    $('#EndUnit')
        .addvalidation_errorElement("#errorUnit")
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(2);

    $('#btnDisplay')
        .addvalidation_errorElement("#errorbtnDisplay"); //E303

}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    sortTable.getSortingTable("mansiontable");

    $('#StartNum, #EndNum').on('change', function () {

        const $this = $(this), $start = $('#StartNum').val(), $end = $('#EndNum').val()

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            StartAge: $start,
            EndAge: $end
        };

        if (model.StartAge && model.EndAge) {
            if (Number(model.StartAge) < Number(model.EndAge)) {
                $("#StartNum").hideError();
                $("#EndNum").hideError();
                $("#EndNum").focus();
                return;
            }

        }

    });

    $('#StartUnit, #EndUnit').on('change', function () {
        const $this = $(this), $start1 = $('#StartUnit').val(), $end1 = $('#EndUnit').val()

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            StartUnit: $start1,
            EndUnit: $end1
        };
        if (model.StartUnit && model.EndUnit) {
            if (Number(model.StartUnit) > Number(model.EndUnit)) {
                $("#StartUnit").showError(common.getMessage('E113'));
                //$("#EndUnit").showError(this.getMessage('E113'));
                $("#StartUnit").focus();
                return;
            }
            else {
                $("#StartUnit").hideError();
                $("#EndUnit").hideError();
                $("#EndUnit").focus();
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
        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件");
        $('#mansiontable tbody').empty();

        const $Apartment = $("#txtApartment").val().trim(), $StartAge = $("#StartNum").val(), $EndAge = $('#EndNum').val(),
            $StartUnit = $("#StartUnit").val(), $EndUnit = $("#EndUnit").val()

        var cityGPCD_check = '';
        var gp_length = 0;
        $('.node-parent:checkbox:checked').each(function () {
            cityGPCD_check +=  $(this).val() + ',' ;
            gp_length += 1;
        });
       
        var cityCD_check = '';
        var city_lenght = 0;
        $('.node-item:checkbox:checked').each(function () {
            cityCD_check += $(this).val() + ',';
            city_lenght += 1;
        });
        
        let model = {
            Apartment: $Apartment,
            StartAge: Get_FT_Age($EndAge, 'F'),
            EndAge: Get_FT_Age($StartAge, 'T'),
            StartUnit: $StartUnit,
            EndUnit: $EndUnit,
            CityCD: cityCD_check.slice(0,-1),
            CityGPCD: cityGPCD_check.slice(0, -1)
        };
        
        if (model.Apartment == "" && model.StartAge == "" && model.EndAge == "" && model.StartUnit == "" && model.EndUnit == "" && model.CityCD == "" && model.CityGPCD == "") {
            $('#btnDisplay').showError(common.getMessage('E303'));
            $("#txtApartment").focus();
        }
        else {
            $('#btnDisplay').hideError();
            GetM_MansionList(model, $form);
        }
            
    });

    $('#btnCSVDownload').on('click', function () {
        $form = $('#form1').hideChildErrors();
        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件");
        $('#mansiontable tbody').empty();

        const $Apartment = $("#txtApartment").val().trim(), $StartAge = $("#StartNum").val(), $EndAge = $('#EndNum').val(),
            $StartUnit = $("#StartUnit").val(), $EndUnit = $("#EndUnit").val()

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

        let model = {
            Apartment: $Apartment,
            StartAge: Get_FT_Age($EndAge, 'F'),
            EndAge: Get_FT_Age($StartAge, 'T'),
            StartUnit: $StartUnit,
            EndUnit: $EndUnit,
            CityCD: cityCD_check.slice(0, -1),
            CityGPCD: cityGPCD_check.slice(0, -1)
        };

        if (model.Apartment == "" && model.StartAge == "" && model.EndAge == "" && model.StartUnit == "" && model.EndUnit == "" && model.CityCD == "" && model.CityGPCD == "") {
            $('#btnDisplay').showError(common.getMessage('E303'));
            $("#txtApartment").focus();
        }
        else {
            $('#btnDisplay').hideError();
            GetM_MansionList(model, $form);

            common.callAjax(_url.generate_CSV1, model,
                function (result) {
                    //sucess
                    var table_data = result.data;

                    var csv = common.getJSONtoCSV(table_data);
                    if (!(csv == "ERROR")) {
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
                        downloadLink.download = "マンション一覧_" + dateString + ".csv";

                        document.body.appendChild(downloadLink);
                        downloadLink.click();
                        document.body.removeChild(downloadLink);
                    }
                    else {
                        $('#site-error-modal').modal('show');
                    }
                }
            )
            common.callAjax(_url.generate_CSV2, model,
                function (result) {
                    //sucess
                    var table_data = result.data;

                    var csv = common.getJSONtoCSV(table_data);
                    if (!(csv == "ERROR")) {
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
                        downloadLink.download = "マンション最寄り駅一覧_" + dateString + ".csv";

                        document.body.appendChild(downloadLink);
                        downloadLink.click();
                        document.body.removeChild(downloadLink);
                    }
                    else {
                        $('#site-error-modal').modal('show');
                    }
                }
            )
            common.callAjax(_url.generate_CSV3, model,
                function (result) {
                    //sucess 
                    var table_data = result.data;

                    var csv = common.getJSONtoCSV(table_data);
                    if (!(csv == "ERROR")) {
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
                        downloadLink.download = "マンション検索用語一覧_" + dateString + ".csv";

                        document.body.appendChild(downloadLink);
                        downloadLink.click();
                        document.body.removeChild(downloadLink);
                    }
                    else {
                        $('#site-error-modal').modal('show');
                    }
                }
            )

        }

        
    });

    $('#btnSignUp').on('click', function () {
        $form = $('#form1').hideChildErrors();
        window.location.href = common.appPath + '/t_mansion_new/Index';
    });

    $(document).on('click', '.tree li  input[type="checkbox"]', function () {
        $(this).closest('li').find('ul input[type="checkbox"]').prop('checked', $(this).is(':checked'));
    }).on('click', '.node-item', function () {
        var parentNode = $(this).parents('.tree ul');
        if ($(this).is(':checked')) {
            parentNode.find('li .node-parent').prop('checked', true);
        } else {
            var elements = parentNode.find('ul input[type="checkbox"]:checked');
            if (elements.length == 0) {
                parentNode.find('li .node-parent').prop('checked', false);
            }
        }
    });
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

function GetM_MansionList(model, $form) {
    common.callAjaxWithLoading(_url.getM_MansionList, model, this, function (result) {
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
        for (var i = 0; i < data.length; i++) {
            html += '<tr>\
            <td class= "text-end" > ' + (i + 1) + '</td>\
            <td>' + data[i]["マンションCD"] + '</td >\
            <td> <a href="#"  onclick="l_logfunction(this.id)" class="text-heading font-semibold text-decoration-underline" id='+ data[i]["マンションCD"] + '>' + data[i]["マンション名"] + '</a></td>\
            <td>' + data[i]["住所"] + ' </td>\
            <td>' + data[i]["築年月"] + '</td>\
            <td class="text-end">' + data[i]["築年数"] + '</td>\
            <td class="text-end">' + data[i]["総戸数"] + '</td>\
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
    $('#mansiontable tbody').append(html);
   
}

function l_logfunction(id) {
    let model = {
        LogDateTime: null,
        LoginKBN: null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: null,
        Processing: null,
        Remarks: null,
        MansionCD: id
    }
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
                alert("go to t_mansion")
                //window.location.href = common.appPath + '/t_mansion/Index?MansionCD=' + model.MansionCD;              
            }
            if (result && !result.isOK) {

            }
        });
}