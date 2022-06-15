const _url = {};

$(function () {
    _url.getBankListByBankWord = common.appPath + '/t_reale_new/GetBankListByBankWord';
    _url.getBankBranchListByBankCD = common.appPath + '/t_reale_new/GetBankBranchListByBankCD';
    _url.getBankBranchListByBranchWord = common.appPath + '/t_reale_new/GetBankBranchListByBranchWord';
    setValidation();
    addEvents();

    
    createBloodhound();
    setTypeahead('#SourceBankName', 'tt-bankcd', bloodhoundSuggestions);
});

function setValidation() {
    $('#REName')
        .addvalidation_errorElement("#errorREName")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte()
        .addvalidation_doublebyte();

    $('#REKana')
        .addvalidation_errorElement("#errorREKana")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte()
        .addvalidation_doublebyte()
        .addvalidation_doublebyte_kana();

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
        .addvalidation_maxlengthCheck(50);

    $('#TownName')
        .addvalidation_errorElement("#errorTownName")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(50);

    $('#Address1')
        .addvalidation_errorElement("#errorAddress1")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(50);

    $('#BusinessHours')
        .addvalidation_errorElement("#errorBusinessHours")
        .addvalidation_maxlengthCheck(50)

    $('#CompanyHoliday')
        .addvalidation_errorElement("#errorCompanyHoliday")
        .addvalidation_maxlengthCheck(50)

    $('#PICName')
        .addvalidation_errorElement("#errorPICName")
        .addvalidation_maxlengthCheck(50)
        .addvalidation_doublebyte();
    $('#PICKana')
        .addvalidation_errorElement("#errorPICKana")
        .addvalidation_maxlengthCheck(50)
        .addvalidation_doublebyte()
        .addvalidation_doublebyte_kana();

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
        .addvalidation_maxlengthCheck(15)
        .addvalidation_singlebyte()
        .addvalidation_custom("customValidation_checkContactAddress");

    $('#SourceBankName')
        .addvalidation_errorElement("#errorSourceBankName")
        .addvalidation_reqired();

    $('#SourceBranchName')
        .addvalidation_errorElement("#errorSourceBranchName")
        .addvalidation_reqired();

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

    
}

function addEvents() {
    //郵便番号
    $('#ZipCode1,#ZipCode2').on('change', function () {
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
                        $('#CityCD').val(data.CityCD)
                    }
                    if (data.TownCD) {
                        $('#TownCD').val(data.TownCD)
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
        if (!$zipCode1 && !$zipCode2) {
            $('#CityCD').val("");
            $('#TownCD').val("");
        }
    });

    
    $('#SourceBankName').on('change', function () {       
        $(this).hideError();
        const $SourceBankCD = $('.tt-bankcd');
        if ($SourceBankCD.get().length === 1) {
            displayBankList($SourceBankCD.text());
        }
        else if ($SourceBankCD.get().length === 0) {
            $(this).showError(common.getMessage("E305"));
        }
    }).on('typeahead:selected', function (evt, data) {
         displayBankList(data.Value);
    });

    $('#SourceBranchName').on('change', function () {
        $(this).hideError();
        const $SourceBankBranchCD = $('.tt-branchcd');
        if ($SourceBankBranchCD.get().length === 0) {
            $(this).showError(common.getMessage("E306"));
        }
    }).on('typeahead:selected', function (evt, data) {
        $('#SourceBranchCD').val(data.Value)
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

function setTypeahead(selector,className,suggestions) {
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
                    return '<div>' + data.DisplayText + '<span class=' + className+' style="display:none;">' + data.Value + '</span></div>';
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

    //common.callAjax(_url.getBankBranchListByBankCD, { bankCD },
    //    function (result) {
    //        if (result && result.isOK) {
    //            const dataArray = JSON.parse(result.data);
    //            const length = dataArray.length;
    //            if (length > 0) {
    //                setTypeahead('#SourceBranchName', 'tt-branch', bloodhoundSuggestions_branch);
    //               common.hideLoading();
    //            }
    //        }
    //    },
    //    function () {
    //        common.hideLoading(null, 0);
    //   }
    //);
}