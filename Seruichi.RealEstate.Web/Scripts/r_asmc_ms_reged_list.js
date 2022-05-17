const _url = {};
let Radio_Checkbox = 0;
$(function () {
    setValidation();
    _url.Get_DataList = common.appPath + '/r_asmc_ms_reged_list/Get_DataList';
    _url.Get_Rating = common.appPath + '/r_asmc_ms_reged_list/Get_Rating';
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

    $('#btnDisplay')
        .addvalidation_errorElement("#errorbtnDisplay"); //E303

}
function addEvents() {
    common.bindValidationEvent('#form1', '');
    //Bind_Rating("[]");
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

    $('#RadioCheck').on('change', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        this.value = this.checked ? 1 : 0;
        Radio_Checkbox = this.value;
       // alert(this.value);
        //Get_Rating_List(this.value);
    });

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#r_table_List tbody').empty();

        const $MansionName = $("#MansionName").val(), $StartYear = $("#StartYear").val(), $EndYear = $('#EndYear').val()
        var cityCD_check = '';
        var city_lenght = 0;
        $('.node-item:checkbox:checked').each(function () {
            cityCD_check += $(this).val() + ',';
            city_lenght += 1;
        });

        var Rdo_Rating = '';
        if (Radio_Checkbox == 1)
        {
          $('.form-check-input:radio:checked').each(function () {
            Rdo_Rating = $(this).val();
            });
        }

        let model = {
            MansionName: $MansionName,
            StartYear: Get_FT_Age($EndYear, 'F'),
            EndYear: Get_FT_Age($StartYear, 'T'),
            CityCD: cityCD_check,
            Radio_Rating: Rdo_Rating
        };
        Get_DataList(model, $form);

    });
}
function Get_Rating_List(checked) {
    if (checked == 1) {
        let model = {};
        common.callAjax(_url.Get_Rating, model, function (result) {
            if (result && result.isOK) {
                Bind_Rating(result.data);
            }
            if (result && !result.isOK) {
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                }
            }
        });
    }
    else
    {
        Bind_Rating("[]");
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
            html += '<tr>\
            <td class= "text-end" > ' + (i + 1) + '</td>\
            <td> <a class="text-heading font-semibold text-decoration-underline" href="#"  onclick="l_logfunction(this.id)" id='+ data[i]["マンションCD"] +'>'+ data[i]["マンション名"] + '</a> </td>\
            <td> <a class="text-heading font-semibold">'+ data[i]["住所"] + '</a> </td>\
            <td> '+ data[i]["登録日"] + '</td>\
            <td>\
            <div class="d-flex flex-row mt-2">\
            <div class="text-danger mb-1 me-2">\
           <i class="fa fa-star"></i> <i class="fa fa-star"></i> <i class="fa fa-star"></i> <i class="fa fa-star"></i> <i class="fa fa-star"></i>\
           </div>\
           </div>\
           </td>\
          </tr>'
        }
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

function Bind_Rating(result) {
    alert(result);
    $('#DivRadio').empty();
    let data = JSON.parse(result);
    let html_Rating = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (data[i]["Rating"] == "4") {

                _Rating = '<span class="fa fa-star" data-rating="1"></span>\
                       <span class="fa fa-star" data-rating="2" ></span >\
                       <span class="fa fa-star" data-rating="3"></span>\
                       <span class="fa fa-star" data-rating="4"></span>\
                       <span class="fa fa-star-o" data-rating="5"></span>';

            }
            if (data[i]["Rating"] == "3") {
                _Rating = '<span class="fa fa-star" data-rating="1"></span>\
                      <span class="fa fa-star" data-rating="2"></span>\
                      <span class="fa fa-star" data-rating="3"></span>\
                      <span class="fa fa-star-o" data-rating="4"></span>\
                      <span class="fa fa-star-o" data-rating="5"></span>'
            }
            if (data[i]["Rating"] == "2") {
                _Rating = '<span class="fa fa-star" data-rating="1"></span>\
                       <span class="fa fa-star" data-rating="2"></span>\
                       <span class="fa fa-star-o" data-rating="3"></span>\
                       <span class="fa fa-star-o" data-rating="4"></span>\
                       <span class="fa fa-star-o" data-rating="5"></span>'
            }
            if (data[i]["Rating"] == "1") {
                _Rating = '<span class="fa fa-star" data-rating="1"></span>\
                       <span class="fa fa-star-o" data-rating="2"></span>\
                       <span class="fa fa-star-o" data-rating="3"></span>\
                       <span class="fa fa-star-o" data-rating="4"></span>\
                       <span class="fa fa-star-o" data-rating="5"></span>'

            }

            if (data[i]["Rating"] == "0") {
                _Rating = '<span class="fa fa-star-o" data-rating="1"></span>\
                       <span class="fa fa-star-o" data-rating="2"></span>\
                       <span class="fa fa-star-o" data-rating="3"></span>\
                       <span class="fa fa-star-o" data-rating="4"></span>\
                       <span class="fa fa-star-o" data-rating="5"></span>'

            }
     

            html_Rating += '<div class="ms-6">\
                <input class="form-check-input" type = "radio" name = "Radios" id = "RadioCheck" value = '+ data[i]["Rating"] + ' >\
                <span class="text-danger">'+_Rating+
               '</span> <span><small>以上</small></span>\
                </div>'
        }
        
    }
    else {

        html_Rating= '<div class="ms-6">\
                     <input class="form-check-input" type="radio" name="Radios" id="RadioRating" value="0">\
                     <span class="text-danger">\
                     <span class="fa fa-star-o" data-rating="1"></span>\
                     <span class="fa fa-star-o" data-rating="2"></span>\
                     <span class="fa fa-star-o" data-rating="3"></span>\
                     <span class="fa fa-star-o" data-rating="4"></span>\
                     <span class="fa fa-star-o" data-rating="5"></span>\
                     </span><span><small>以上</small></span>\
                     </div>'
 
           }
$('#DivRadio').append(html_Rating);
}