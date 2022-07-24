const _url = {};

$(function () {
    _url.getTemplateOptContent = common.appPath + '/r_asmc_set_ms/GetTemplateOptContent';
    _url.gotoComfirm = common.appPath + '/r_asmc_set_ms/GotoComfirm';

    setValidation();
    addEvents();
    $('#PriorityCheck').change();
    $('#Rate').focus();

    const errorJson = $('#ValidationResult').val();
    if (errorJson) {
        common.setValidationErrors(JSON.parse(errorJson));
        const target = $('body').getInvalidItems().get(0);
        if (target) {
            const id = $(target).parents('.tab-pane').attr('aria-labelledby');
            if (id) {
                $('#' + id).click();
                $(target).focus();
            }
        }
    }
});

function setValidation() {
    //買取利回り設定
    $('#Rate')
        .addvalidation_errorElement("#errorRate")
        .addvalidation_reqired()
        .addvalidation_numeric(3, 2, true);
    //家賃設定
    $('#RentHigh')
        .addvalidation_errorElement("#errorRentHigh")
        .addvalidation_money(10);
    $('#RentLow')
        .addvalidation_errorElement("#errorRentLow")
        .addvalidation_money(10)
        .addvalidation_custom('customValidation_checkRentLow');
    //オプション 1:総戸数
    $('#Opt1_Value')
        .addvalidation_errorElement("#errorOpt1")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
    //オプション 2:所在階
    $('input[name="Opt2_OptionKBN"]:radio')
        .addvalidation_errorElement("#errorOpt2")
        .addvalidation_reqired();
    $('#Opt2_IncDecRate')
        .addvalidation_errorElement("#errorOpt2")
        .addvalidation_reqired(true)
        .addvalidation_numeric(3, 2, true);
    //オプション 4:専有面積
    $('#Opt4_Value')
        .addvalidation_errorElement("#errorOpt4")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
    //オプション 8:部屋数
    $('#Opt8_Value')
        .addvalidation_errorElement("#errorOpt8")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
    $('input[name="Opt8_NotApplicableFLG"]:radio')
        .addvalidation_errorElement("#errorOpt8")
        .addvalidation_reqired();
    $('#Opt8_IncDecRate')
        .addvalidation_errorElement("#errorOpt8")
        .addvalidation_reqired(true)
        .addvalidation_numeric(3, 2, true);
    //オプション 9:バス・トイレ
    $('input[name="Opt9_CategoryKBN"]:radio')
        .addvalidation_errorElement("#errorOpt9")
        .addvalidation_reqired();
    $('input[name="Opt9_NotApplicableFLG"]:radio')
        .addvalidation_errorElement("#errorOpt9")
        .addvalidation_reqired();
    $('#Opt9_IncDecRate')
        .addvalidation_errorElement("#errorOpt9")
        .addvalidation_reqired(true)
        .addvalidation_numeric(3, 2, true);
    //オプション 11:賃貸状況
    $('input[name="Opt11_CategoryKBN"]:radio')
        .addvalidation_errorElement("#errorOpt11")
        .addvalidation_reqired();
    $('input[name="Opt11_NotApplicableFLG"]:radio')
        .addvalidation_errorElement("#errorOpt11")
        .addvalidation_reqired();
    $('#Opt11_IncDecRate')
        .addvalidation_errorElement("#errorOpt11")
        .addvalidation_reqired(true)
        .addvalidation_numeric(3, 2, true);
    //オプション 12:管理状況
    $('input[name="Opt12_CategoryKBN"]:radio')
        .addvalidation_errorElement("#errorOpt12")
        .addvalidation_reqired();
    $('input[name="Opt12_NotApplicableFLG"]:radio')
        .addvalidation_errorElement("#errorOpt12")
        .addvalidation_reqired();
    $('#Opt12_IncDecRate')
        .addvalidation_errorElement("#errorOpt12")
        .addvalidation_reqired(true)
        .addvalidation_numeric(3, 2, true);
    //オプション 13:売却希望時期
    $('input[name="Opt13_CategoryKBN"]:radio')
        .addvalidation_errorElement("#errorOpt13")
        .addvalidation_reqired();
    //備考
    $('#Remark')
        .addvalidation_errorElement("#errorRemark")
        .addvalidation_singlebyte_doublebyte();

    $('#tableOpt')
        .addvalidation_errorElement("#errorTableOpt");

}

function addEvents() {

    //共通チェック処理
    common.bindValidationEvent('#pills-home');
    common.bindValidationEvent('#pills-rent');
    common.bindValidationEvent('#pills-opt', ':not(input[type="radio"])');
    common.bindValidationEvent('#pills-attribute');

    //オプション-区分選択ボタン
    $('.js-opt-button').on('click', function () {
        const $this = $(this);
        const kbn = $this.data('cap-optkbn');

        $('.js-opt-error').hide();
        $('#errorOpt' + kbn).show();

        $('#btnOptReset').prop('disabled', kbn === 5 || kbn === 6 || kbn === 10);
        $('#hdnSelectedKBN').val(kbn);
    });

    //オプション-ドロップダウンリスト
    $('.js-opt-li').on('click', function () {
        const $this = $(this);
        const $target = $this.parent().parent().parent().find('.dropdown-toggle');
        $target.data('cap-value-selected', $this.data('cap-value')).text($this.text());
    });

    //オプション-2:所在階
    $('input[name="Opt2_OptionKBN"]:radio').on('change', function () {
        if ($('input[name="Opt2_OptionKBN"]:radio:checked').val() === "3") {
            $('#Opt2_IncDecRate').prop('disabled', false).addvalidation_reqired(true);
        }
        else {
            $('#Opt2_IncDecRate').val('').prop('disabled', true).removeValidation_required().hideError();
        }
    });

    //オプション-8:部屋数
    //オプション-9:バス・トイレ
    //オプション-11:賃貸状況
    //オプション-12:管理状況
    function switch_NotApplicableFLG(optionkbn) {
        const radiogroup = 'Opt' + optionkbn + '_NotApplicableFLG';
        const $IncDecRate = $('#Opt' + optionkbn + '_IncDecRate');

        if ($('input[name="' + radiogroup + '"]:radio:checked').val() === "0") {
            $IncDecRate.prop('disabled', false).addvalidation_reqired(true);
        }
        else {
            $IncDecRate.val('').prop('disabled', true).removeValidation_required().hideError();
        }
    }

    $('input[name="Opt8_NotApplicableFLG"]:radio').on('change', function () {
        switch_NotApplicableFLG(8);
    });

    $('input[name="Opt9_NotApplicableFLG"]:radio').on('change', function () {
        switch_NotApplicableFLG(9);
    });

    $('input[name="Opt11_NotApplicableFLG"]:radio').on('change', function () {
        switch_NotApplicableFLG(11);
    });

    $('input[name="Opt12_NotApplicableFLG"]:radio').on('change', function () {
        switch_NotApplicableFLG(12);
    });

    //オプション-追加
    $('#btnOptAdd').on('click', function () {
        const kbn = parseInt($('#hdnSelectedKBN').val());

        const panel_selector = '#optpanel' + kbn + '-tab';
        const $panel = $(panel_selector).hideChildErrors();

        if (!common.checkValidityOnSave(panel_selector)) {
            common.setFocusFirstError($panel);
            return false;
        }

        const rowData = getOptRowData(kbn);

        $('#tableOpt tbody').append(get_trHtml(rowData));
        sort_optTable('#tableOpt');

        $('#btnOptReset').click();
    });

    //オプション-リセット
    $('#btnOptReset').on('click', function () {
        const kbn = parseInt($('#hdnSelectedKBN').val());

        const panel_selector = '#optpanel' + kbn + '-tab';
        $panel = $(panel_selector).hideChildErrors();

        $panel.find('input[type="text"]').val('');
        $panel.find('input[type="radio"]').prop('checked', false).change();
    });

    //オプション-×ボタン
    $(document).on('click', '.js-opt-xbtn', function () {
        $(this).parents('tr').remove();
    });

    //有効期限
    $('.js-btn-addmonth').on('click', function () {
        const addend = $(this).data('cap-addend');

        let d = new Date();
        if (addend) {
            d.setMonth(d.getMonth() + addend);
        }

        var formatted = `${d.getFullYear()}-${(d.getMonth() + 1).toString().padStart(2, '0')}-${d.getDate().toString().padStart(2, '0')}`.replace(/\n|\r/g, '');
        $('#ExpDate').val(formatted);
    });

    //優先度CheckBox
    $('#PriorityCheck').on('change', function () {
        if (!$(this).prop('checked')) {
            $('#Priority').val('0');
            $('.star-rating .fa').removeClass('fa-star').addClass('fa-star-o');
        }
    });

    //☆☆☆☆☆
    $('.star-rating .fa').on('click', function () {
        if (!$('#PriorityCheck').prop('checked')) return;

        const rating = $(this).data('rating');
        $('#Priority').val(rating);

        $('.star-rating .fa').each(function () {
            if ($(this).data('rating') <= rating) {
                $(this).removeClass('fa-star-o').addClass('fa-star');
            } else {
                $(this).removeClass('fa-star').addClass('fa-star-o');
            }
        });
    });

    //確認
    $('#btnShowConfirmation').on('click', function () {
        const notApplicableFlg = $('#NotApplicableFlg').prop('checked') ? 1 : 0;

        if (notApplicableFlg === 0) {
            const $pills_home = $('#pills-home').hideChildErrors();
            const $pills_rent = $('#pills-rent').hideChildErrors();
            const $pills_attr = $('#pills-attribute').hideChildErrors();
            let hasError = false;

            if ($())
                //入力チェック-買取利回り率設定
                $('#pills-home :input:not(button):not(:disabled):not([readonly])').each(function () {
                    if (!common.checkValidityInput(this)) {
                        hasError = true;;
                    }
                });
            if (hasError) {
                $('#pills-home-tab').click();
                common.setFocusFirstError($pills_home);
                return false;
            }
            //入力チェック-家賃設定
            $('#pills-rent :input:not(button):not(:disabled):not([readonly])').each(function () {
                if (!common.checkValidityInput(this)) {
                    hasError = true;;
                }
            });
            if (hasError) {
                $('#pills-rent-tab').click();
                common.setFocusFirstError($pills_rent);
                return false;
            }
            //入力チェック-属性設定
            $('#pills-attribute :input:not(button):not(:disabled):not([readonly])').each(function () {
                if (!common.checkValidityInput(this)) {
                    hasError = true;;
                }
            });
            if (hasError) {
                $('#pills-attribute-tab').click();
                common.setFocusFirstError($pills_attr);
                return false;
            }
        }

        const callback = function () {
            const fd = new FormData();
            fd.append('MansionCD', $('#MansionCD').text());
            fd.append('MansionName', $('#MansionName').text());
            fd.append('Address', $('#Address').text());
            fd.append('RealEstateCount', $('#RealEstateCount').text());
            fd.append('PrecedenceFlg', $('#PrecedenceFlg').prop('checked') ? 1 : 0);
            fd.append('NotApplicableFlg', notApplicableFlg);
            fd.append('ValidFLG', $('#hdnValidFLG').val());
            fd.append('ExpDate', $('#ExpDate').val());
            fd.append('Priority', $('#Priority').val());
            fd.append('Remark', $('#Remark').val());
            fd.append('REStaffCD', $('#REStaffCD').val());
            fd.append('REStaffName', $('#REStaffCD option:selected').text());
            if (notApplicableFlg === 0) {
                fd.append('Rate', $('#Rate').val());
                fd.append('RentHigh', $('#RentHigh').val());
                fd.append('RentLow', $('#RentLow').val());
                fd.append('RECondManOptJson', JSON.stringify(getRECondManOptionList()));
            }

            const form = document.createElement("form");
            document.body.append(form);
            common.callSubmit(form, _url.gotoComfirm, fd);
        }

        if (notApplicableFlg === 1) {
            common.showConfirmPopup('査定買取不可にチェックされている為、買取利回り率・家賃・オプションの情報は登録されません！よろしいですか？', callback);
        } else {
            callback();
        }
    });

    //破棄
    $('#btnDiscard').on('click', function () {
        window.location.reload();
    });

    //テンプレート読み込み
    $('#btnLoadTemplate').on('click', function () {
        const model = {
            templateNo: $('#TemplateSelect').val()
        }

        common.showLoading();
        common.callAjax(_url.getTemplateOptContent, model,
            function (result) {
                if (result) {
                    $('#tableOptContainer').empty().append(result);
                }
                common.hideLoading();
                $('#TemplateModal').modal('hide');
            },
            function () {
                common.hideLoading(null, 0);
            }
        );

    });
}

function getOptRowData(kbn) {
    const rowData = {
        OptionKBN: kbn,
        OptionSEQ: 0,
        CategoryKBN: parseInt($('input[name="Opt' + kbn + '_CategoryKBN"]:radio:checked').val()),
        Value1: parseInt($('#Opt' + kbn + '_Value').val()),
        HandlingKBN1: $('#Opt' + kbn + '_handlingOptionKBN').data('cap-value-selected'),
        NotApplicableFLG: parseInt($('input[name="Opt' + kbn + '_NotApplicableFLG"]:radio:checked').val()),
        IncDecRate: Number($('#Opt' + kbn + '_IncDecRate').val())
    };
    if (isNaN(rowData.CategoryKBN)) rowData.CategoryKBN = 0;
    if (isNaN(rowData.Value1)) rowData.Value1 = 0;
    if (isNaN(rowData.HandlingKBN1)) rowData.HandlingKBN1 = 0;
    if (isNaN(rowData.NotApplicableFLG)) rowData.NotApplicableFLG = 1;
    if (isNaN(rowData.IncDecRate)) rowData.IncDecRate = 0;

    if (kbn === 2) {
        rowData.OptionKBN = parseInt($('input[name="Opt2_OptionKBN"]:radio:checked').val());
        if (isNaN(rowData.OptionKBN)) {
            rowData.OptionKBN = 0;
        }
        if (rowData.OptionKBN === 3) {
            rowData.NotApplicableFLG = 0;
        }
    }

    if (kbn === 8) {
        rowData.HandlingKBN1 = 5;
    }

    return rowData;
}

function getRECondManOptionList() {
    let table = document.querySelector('#tableOpt');
    let array = [];

    for (let r = 1; r < table.rows.length; r++) {
        const rowData = JSON.parse(table.rows[r].cells[3].textContent);
        if (rowData.OptionKBN !== 0) {
            rowData.OptionSEQ = r;
            array[array.length] = rowData;
        }
    }

    return array;
}

function customValidation_checkRentLow(e) {
    const val1 = $('#RentLow').val().replace(/,/g, '');
    const val2 = $('#RentHigh').val().replace(/,/g, '');

    if (val1 && val2) {
        if (Number(val1) > Number(val2)) {
            $('#RentLow').showError(common.getMessage('E113'));
            return false;
        }
    }

    return true;
}
