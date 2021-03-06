const _url = {};
$(function () {
    _url.get_t_reale_CompanyInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyInfo';
    _url.get_t_reale_CompanyCountingInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyCountingInfo';

    _url.getBankListByBankWord = common.appPath + '/t_reale_new/GetBankListByBankWord';
    _url.getBankBranchListByBranchWord = common.appPath + '/t_reale_new/GetBankBranchListByBranchWord';
    _url.checkZipCode = common.appPath + '/t_reale_new/CheckZipCode';

    _url.checkAll = common.appPath + '/t_reale_profile/CheckAll';

    _url.update_t_reale_profile = common.appPath + '/t_reale_profile/Update_t_reale_profile';

    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_list').addClass('font-bold text-underline');

    $('#REName').focus();

    setValidation();
    addEvents();

    createBloodhound();
    setTypeahead('#SourceBankName', 'tt-bankcd', bloodhoundSuggestions);
});

function setValidation() {
    $('#REName')
        .addvalidation_errorElement("#errorREName")
        .addvalidation_reqired()
        .addvalidation_doublebyte()
        .addvalidation_singlebyte_doublebyte();

    $('#REKana')
        .addvalidation_errorElement("#errorREKana")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte()
        //.addvalidation_doublebyte()
        .addvalidation_doublebyte_namae_kana();

    $('#President')
        .addvalidation_errorElement("#errorPresident")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte()
        .addvalidation_doublebyte();

    $('#ZipCode1')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_singlebyte_number();

    $('#ZipCode2')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_singlebyte_number();

    $('#PrefCD')
        .addvalidation_errorElement("#errorPrefCD")
        .addvalidation_reqired();

    $('#CityName')
        .addvalidation_errorElement("#errorCityName")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte();

    $('#TownName')
        .addvalidation_errorElement("#errorTownName")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte();

    $('#Address1')
        .addvalidation_errorElement("#errorAddress1")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte();

    $('#BusinessHours')
        .addvalidation_errorElement("#errorBusinessHours")
        .addvalidation_singlebyte_doublebyte();

    $('#CompanyHoliday')
        .addvalidation_errorElement("#errorCompanyHoliday")
        .addvalidation_singlebyte_doublebyte();

    $('#PICName')
        .addvalidation_errorElement("#errorPICName")
        .addvalidation_singlebyte_doublebyte()
        .addvalidation_doublebyte();

    $('#PICKana')
        .addvalidation_errorElement("#errorPICKana")
        .addvalidation_singlebyte_doublebyte()
        .addvalidation_doublebyte_namae_kana();

    $('#HousePhone')
        .addvalidation_errorElement("#errorHousePhone")
        .addvalidation_reqired()
        .addvalidation_singlebyte_number()
        .addvalidation_maxlengthCheck(15);

    $('#Fax')
        .addvalidation_errorElement("#errorFax")
        .addvalidation_singlebyte_number()
        .addvalidation_maxlengthCheck(15);

    $('#MailAddress')
        .addvalidation_errorElement("#errorMailAddress")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(100)
        .addvalidation_singlebyte()
        .addvalidation_custom("customValidation_checkContactAddress");

    $('#SourceBankName')
        .addvalidation_errorElement("#errorSourceBankName")
    // .addvalidation_reqired();

    $('#SourceBranchName')
        .addvalidation_errorElement("#errorSourceBranchName")
    // .addvalidation_reqired();

    $('#SourceAccountType')
        .addvalidation_errorElement("#errorSourceAccountType")
        .addvalidation_reqired();

    $('#SourceAccount')
        .addvalidation_errorElement("#errorSourceAccount")
        .addvalidation_reqired()
        .addvalidation_singlebyte_number()
        .addvalidation_maxlengthCheck(8);

    $('#SourceAccountName')
        .addvalidation_errorElement("#errorSourceAccountName")
        .addvalidation_reqired()
        .addvalidation_singlebyte_kana()
        .addvalidation_maxlengthCheck(50);

    $('#LicenceNo2')
        .addvalidation_errorElement("#errorLicenceNo2")
        .addvalidation_singlebyte_number()
        .addvalidation_maxlengthCheck(2);

    $('#LicenceNo3')
        .addvalidation_errorElement("#errorLicenceNo3")
        .addvalidation_singlebyte_number()
        .addvalidation_maxlengthCheck(10);

    $('#CourseCD')
        .addvalidation_errorElement("#errorCourseCD")
        .addvalidation_reqired();

    $('#NextCourseCD')
        .addvalidation_errorElement("#errorNextCourseCD")
        .addvalidation_reqired();

    $('#JoinedDate')
        .addvalidation_errorElement("#errorJoinedDate")
        .addvalidation_datecheck();

    $('#InitialFee')
        .addvalidation_errorElement("#errorInitialFee")
        .addvalidation_reqired()
        .addvalidation_money(9);

    $('#Remark')
        .addvalidation_errorElement("#errorRemark")
        .addvalidation_singlebyte_doublebyte();

    $('#txtPassword')
        .addvalidation_errorElement("#errortxtPassword")
        .addvalidation_reqired()
        .addvalidation_singlebyte()
        .addvalidation_minlengthCheck(8)
        .addvalidation_maxlengthCheck(20);

    $('#txtConfirmPassword')
        .addvalidation_errorElement("#errortxtConfirmPassword")
        .addvalidation_reqired()
        .addvalidation_passwordcompare();

}

function addEvents() {

    $('.container-fluid .card-body').keypress(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
        }
    });

    $(function () { $("#form1").submit(function () { return false; }); });

    common.bindValidationEvent('#form1', ':not(#ZipCode1,#ZipCode2)');

    //郵便番号
    $('#ZipCode1,#ZipCode2').on('change', function () {

        $('#CityName').val("");
        $('#TownName').val("");

        const $this = $(this), $zipCode1 = $('#ZipCode1'), $zipCode2 = $('#ZipCode2')

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            ZipCode1: $zipCode1.val(),
            ZipCode2: $zipCode2.val()
        };

        if (!model.ZipCode1 && !model.ZipCode2) {
            $($zipCode1, $zipCode2).hideError();
            return;
        }

        common.callAjax(_url.checkZipCode, model,
            function (result) {
                if (result && result.isOK) {
                    $($zipCode1).hideError();
                    $($zipCode2).hideError();

                    const data = result.data;

                    if (data.PrefCD) {
                        $('#PrefCD').val(data.PrefCD).hideError();

                    }
                    if (data.CityCD) {
                        $('#CityName').val(data.CityCD)
                    }
                    if (data.TownCD) {
                        $('#TownName').val(data.TownCD)
                    }
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });

    $('#PrefCD').on('change', function () {
        $zipCode1 = $('#ZipCode1').val(), $zipCode2 = $('#ZipCode2').val()
        if ($zipCode1 && $zipCode2) {
            $('#CityName').val("");
            $('#TownName').val("");
        }
    });

    $('#SourceBankName').on('blur', function () {
        const $this = $(this);
        $(this).hideError();
        const $SourceBankCD = $('.tt-bankcd');
        if ($('#SourceBankName').val() === "") {
            $this.showError(common.getMessage('E101'));
        }
        else if ($SourceBankCD.get().length === 1) {
            displayBankList($SourceBankCD.text());
        }
        else if ($SourceBankCD.get().length === 0) {
            const message = common.getMessage('E305');
            $this.showError(common.getMessage('E306'));
        }
    }).on('typeahead:selected', function (evt, data) {
        displayBankList(data.Value);
    });

    $('#SourceBankName').on('change', function () {
        const $SourceBankCD = $('.tt-bankcd');
        if ($SourceBankCD.get().length === 1) {
            displayBankList($SourceBankCD.text());
        }
    }).on('typeahead:selected', function (evt, data) {
        displayBankList(data.Value);
    });

    $('#SourceBranchName').on('blur', function () {
        const $this = $(this);
        $this.hideError();
        const $SourceBankBranchCD = $('.tt-branchcd');
        if ($('#SourceBranchName').val() === "") {
            $this.showError(common.getMessage('E101'));
        }
        else if ($SourceBankBranchCD.get().length === 0) {
            $this.showError(common.getMessage('E306'));
        }
    }).on('typeahead:selected', function (evt, data) {
        $('#SourceBranchCD').val(data.Value)
    });

    $('#SourceBranchName').on('typeahead:selected', function (evt, data) {
        $('#SourceBranchCD').val(data.Value)
    });

    $('#btnConfirmation').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            common.setFocusFirstError($form);
            return false;
        }
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        model.PrefName = $('#PrefCD option:selected').text();
        model.SourceAccountTypeName = $('#SourceAccountType option:selected').text();
        model.CourseName = $('#CourseCD option:selected').text();
        model.LicenceNo1Name = $('#LicenceNo1 option:selected').text();
        model.NextCourseName = $('#NextCourseCD option:selected').text();
        model.Status = "update";
        common.callAjaxWithLoading(_url.checkAll, model, this, function (result) {
            if (result && result.isOK) {
                saveData = model;
                setScreenComfirm(saveData);
                $('#modal_1').modal('show');
            }
            if (result && result.data) {
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    })

    $('#btnRegistration').on('click', function () {
        common.callAjaxWithLoading(_url.update_t_reale_profile, saveData, this, function (result) {
            if (result && result.isOK) {
                //sucess
                $('#modal_1').modal('hide');
                $('#modal_2').modal('show');
            }
            if (result && result.data) {
                //error
                $('#modal_1').modal('hide');
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnCancel').click(function () {
        $('#changecal').modal('show');
    });

    $('#btnSuccessPopup').click(function () {
        //window.location.href = common.appPath + "/t_mansion_list/index";
        window.location.reload();
    });
    $('#btn_reload').click(function () {
        window.location.reload();
    });
}

function customValidation_checkContactAddress(e) {
    const $this = $(e)

    if ($this.val().trim() == "") {
        $this.showError(common.getMessage('E202'));
        return false;
    }
    return true;
}

var bloodhoundSuggestions;
function createBloodhound() {
    bloodhoundSuggestions = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('DisplayText'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //sufficient: 3000,
        remote: {
            url: _url.getBankListByBankWord,
            prepare: function (query, settings) {
                //リモートサーバーに渡すクエリを加工
                //settingsはjQueryの$.ajaxへ渡すオプション
                var data = { searchWord: query };
                settings.type = 'POST';
                settings.contentType = 'application/json';
                settings.xhrFields = {
                    withCredentials: false,
                };
                settings.headers = {
                    'RequestVerificationToken': $("#_RequestVerificationToken").val(),
                };
                settings.data = JSON.stringify(data);
                return settings;
            },
            transform: function (res) {
                return JSON.parse(res.data);
            },
            rateLimitWait: 300
        },
        limiter: 3000,
        name: 'typeaheadSourceCache',
        ttl: 0,
        ajax: {
            cache: false
        }
    });
}

var bloodhoundSuggestions_branch;
function createBloodhound_branch() {
    bloodhoundSuggestions_branch = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('DisplayText'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //sufficient: 3000,
        remote: {
            url: _url.getBankBranchListByBranchWord,
            prepare: function (query, settings) {
                //リモートサーバーに渡すクエリを加工
                //settingsはjQueryの$.ajaxへ渡すオプション
                var data = { searchWord: query, bankCD: $('#SourceBankCD').val() };
                settings.type = 'POST';
                settings.contentType = 'application/json';
                settings.xhrFields = {
                    withCredentials: false,
                };
                settings.headers = {
                    'RequestVerificationToken': $("#_RequestVerificationToken").val(),
                };
                settings.data = JSON.stringify(data);
                return settings;
            },
            transform: function (res) {
                return JSON.parse(res.data);
            },
            rateLimitWait: 300
        },
        limiter: 3000,
        name: 'typeaheadSourceCache',
        ttl: 0,
        ajax: {
            cache: false
        }
    });
}

function setTypeahead(selector, className, suggestions) {
    $(selector).typeahead('destroy').typeahead(
        {
            hint: false,
            highlight: true,
            minLength: 2
        },
        {
            name: 'data',
            display: 'DisplayText',
            source: suggestions,
            templates: {
                suggestion: function (data) {
                    return '<div>' + data.DisplayText + '<span class=' + className + ' style="display:none;">' + data.Value + '</span></div>';
                }
            },
            limit: 3000,
        });
}

function displayBankList(bankCD) {
    $('#SourceBankCD').val(bankCD);
    common.showLoading();
    createBloodhound_branch();
    setTypeahead('#SourceBranchName', 'tt-branchcd', bloodhoundSuggestions_branch);
    common.hideLoading(null, 0);
}

function setScreenComfirm(data) {
    for (key in data) {
        const target = document.getElementById('confirm_' + key);
        if (target) $(target).val(data[key]);
    }
    $('#confirm_PrefCD').val(data.PrefName);
    $('#confirm_SourceAccountType').val(data.SourceAccountTypeName);
    $('#confirm_CourseCD').val(data.CourseName);
    $('#confirm_NextCourseCD').val(data.NextCourseName);
    $('#confirm_LicenceNo1').val(data.LicenceNo1Name);
}

function setScreenComfirm(data) {
    for (key in data) {
        const target = document.getElementById('confirm_' + key);
        if (target) $(target).val(data[key]);
    }
    $('#confirm_PrefCD').val(data.PrefName);
    $('#confirm_SourceAccountType').val(data.SourceAccountTypeName);
    $('#confirm_CourseCD').val(data.CourseName);

    $('#confirm_LicenceNo1').val(data.LicenceNo1Name);

    $('#confirm_NextCourseCD').val(data.NextCourseName);
}