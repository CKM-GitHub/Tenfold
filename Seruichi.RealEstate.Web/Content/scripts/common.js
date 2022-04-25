﻿window.addEventListener('pageshow', function (event) {
    if (event.persisted) {
        window.location.reload();
    }
});

//in Layout.cshtml, the database values are set
const messageConst = {
    E101: '入力が必要です',
    E102: '必ず１つ選択してください',
    E103: '入力された値が正しくありません',
    E104: '入力できない文字です',
    E105: '入力できる桁数を超えています',
    E106: 'システム内部で必要な設定が足りていません',
    E107: '全角文字で入力してください',
    E108: '正しい日付を入力してください',
    E109: 'パスワードが違います',
    E110: '入力すべき文字数を満たしていません',
    E111: '日付の大小が不正です',
    E112: '１つ以上選択してください',

    E201: '査定サービスの対象外地域です',
    E202: 'メールアドレスが入力されていません',
    E203: '既に登録済のメールアドレスです',
    E204: 'メールアドレスを正しく入力してください',
    E205: 'パスワードが入力されていません',
    E206: 'メールアドレスとパスワードの組合せが正しくありません',
    E207: 'この情報での登録はありません',

    E310: '会社IDが入力されていません',
    E311: '会社IDが未登録です',
    E312: 'スタッフIDが入力されていません',
    E313: 'スタッフIDが未入力です。',
}

const commonApiUrl = {
    getMessage: "/api/CommonApi/GetMessage",   
}

const regexPattern = {
    doublebyte: "[^\x01-\x7E\uFF61-\uFF9F]",
    singlebyte: "^[a-zA-Z0-9!-/:-@¥[-`{-~]*$",
    singlebyte_number: "^[0-9]+$",
    singlebyte_numberAlpha: "^[0-9a-zA-Z]+$",
    moeney: "^[0-9,]+$",
    numeric: "^[0-9.]+$",
    dateformat : /^\d{4}-\d{2}-\d{2}$/,
}

const common = {

    appPath: "", //set in Layout.cshtml

    getMessage: function getMessage(id) {
        let msg = messageConst[id];
        if (msg) return messageConst[id];

        common.callAjax(common.appPath + commonApiUrl.getMessage, { MessageID: id }, function (result) {
            if (result && result.isOK) {
                msg = result.data.MessageText1;
                messageConst[id] = msg;
                return msg;
            }
        });
    },

    querySerialize: function querySerialize(data) {
        const encode = window.encodeURIComponent;
        let key, value, type, i, max;
        let query = '';

        for (key in data) {
            value = data[key];
            type = typeof (value) === 'object' && value instanceof Array ? 'array' : typeof (value);
            switch (type) {
                case 'undefined':
                    // only key
                    query += key;
                    break;
                case 'array':
                    // array
                    for (i = 0, max = value.length; i < max; i++) {
                        query += key + '[]';
                        query += '=';
                        query += encode(value[i]);
                        query += '&';
                    }
                    query = query.substr(0, query.length - 1);
                    break;
                case 'object':
                    // hash
                    for (i in value) {
                        query += key + '[' + i + ']';
                        query += '=';
                        query += encode(value[i]);
                        query += '&';
                    }
                    query = query.substr(0, query.length - 1);
                    break;
                default:
                    query += key;
                    query += '=';
                    query += encode(value);
                    break;
            }
            query += '&';
        }
        query = query.substr(0, query.length - 1);
        return '?' + encodeURI(query);
    },

    callAjaxWithLoading: function callAjaxWithLoading(url, model, disableSelector, successCallback, failCallback) {
        common.showLoading(disableSelector);
        $.ajax({
            url: url,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: JSON.stringify(model),
            headers: {
                RequestVerificationToken: $("#_RequestVerificationToken").val(),
            },
            async: true,
            timeout: 30000,
        }).done(function (data) {
            common.hideLoading(disableSelector);
            if (successCallback) {
                successCallback(data);
            }
        }).fail(function (XMLHttpRequest, status, e) {
            common.hideLoading(disableSelector);
            //alert(XMLHttpRequest.status);
            if (failCallback) {
                failCallback();
            }
        });
    },

    callAjax: function callAjax(url, model, successCallback, failCallback) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: JSON.stringify(model),
            headers: {
                RequestVerificationToken: $("#_RequestVerificationToken").val(),
            },
            async: true,
            timeout: 100000,
        }).done(function (data) {
            if (successCallback) {
                successCallback(data);
            }
        }).fail(function (XMLHttpRequest, status, e) {
            //alert(XMLHttpRequest.status);
            if (failCallback) {
                failCallback();
            }
        });
    },  

    showLoading: function showLoading(disableSelector) {
        $loadiong = $('<div id="loadiong"><div class="cv-spinner"><span class="spinner"></span></div></div>');
        $('body').append($loadiong);
        $('#loadiong').fadeIn(300);
        if (disableSelector) $(disableSelector).prop('disabled', true);
    },

    hideLoading: function hideLoading(disableSelector, delaytime) {
        setTimeout(function () {
            if (disableSelector) $(disableSelector).prop('disabled', false);
            $("#loadiong").fadeOut(300).remove();
        }, delaytime || 500);
    },

    getStringByteCount: function getStringByteCount(str) {
        var blob = new Blob([str], { type: 'text/plain' });
        return blob.size;
    },

    replaceDoubleToSingle: function replaceDoubleToSingle(str) {
        return str.replace(/[！-～]/g, function (s) {
            return String.fromCharCode(s.charCodeAt(0) - 0xFEE0);
        });
    },

    replaceSingleToDouble: function replaceSingleToDouble(str) {
        return str.replace(/[!-~]/g, function (s) {
            return String.fromCharCode(s.charCodeAt(0) + 0xFEE0);
        });
    },

    checkValidityInput: function checkValidityInput(ctrl) {
        const $ctrl = $(ctrl);
        const type = $ctrl.attr('type');
        const name = $ctrl.attr('name');
        const isrequirednullorempty1 = $ctrl.attr("data-validation-requirednullorempty1");
        const isrequirednullorempty2 = $ctrl.attr("data-validation-requirednullorempty2");
        const isrequirednullorempty3 = $ctrl.attr("data-validation-requirednullorempty3");

        const isRequired = $ctrl.attr("data-validation-required");
        const isRequiredNotAllowZero = $ctrl.attr("data-validation-not-allow-zero");
        const isSingleDoubleByte = $ctrl.attr("data-validation-singlebyte-doublebyte");
        const isDoubleByte = $ctrl.attr("data-validation-doublebyte");
        const isSingleByte = $ctrl.attr("data-validation-singlebyte");
        const isSingleByteNumber = $ctrl.attr("data-validation-singlebyte-number");
        const isSingleByteNumberAlpha = $ctrl.attr("data-validation-singlebyte-numberalpha");
        const isNumeric = $ctrl.attr("data-validation-numeric");
        const isMoney = $ctrl.attr("data-validation-money");
        const customValidation = $ctrl.attr("data-validation-custom");
        const isDate = $ctrl.attr("data-validation-datecheck");
        const isMaxlengthCheck = $ctrl.attr("data-validation-MaxLength");
        const isDateCompare = $ctrl.attr("data-validation-datecompare");
        const isOneByteCharacter = $ctrl.attr("data-validation-onebyte-character");
        const ischeckboxLenght = $ctrl.attr("data-validation-checkboxlenght");
        const isMaxlengthCheckforsellerlist = $ctrl.attr("data-validation-MaxLengthforsellerlist");       

        let inputValue = "";
        if (type === 'radio') {
            inputValue = $('input[name="' + name + '"]:radio:checked').val();
        }
        else {
            inputValue = $ctrl.val().trimEnd();
            $ctrl.val(inputValue);
        }

        //replace text
        if (isSingleByte || isSingleByteNumber || isSingleByteNumberAlpha || isNumeric || isMoney) {
            inputValue = this.replaceDoubleToSingle(inputValue);
            $ctrl.val(inputValue);
        }

        if (isDoubleByte) {
            inputValue = this.replaceSingleToDouble(inputValue);
            $ctrl.val(inputValue);
        }

        //validation
        if (!isRequired
            && !isSingleDoubleByte
            && !isDoubleByte
            && !isSingleByte && !isSingleByteNumber && !isSingleByteNumberAlpha
            && !isNumeric && !isMoney && !isDate && !isMaxlengthCheck && !isOneByteCharacter && !ischeckboxLenght && !isMaxlengthCheckforsellerlist
            && !customValidation && !isrequirednullorempty1 && !isrequirednullorempty2 && !isrequirednullorempty3) return true;

        if (isRequired) {
            if (!inputValue) {
                if ($ctrl.prop("tagName") === 'SELECT') {
                    if ($ctrl.children('.js-selectable').length > 0) {
                        $ctrl.showError(this.getMessage('E102'));
                        return false;
                    }
                }
                else if (type === 'radio') {
                    $ctrl.showError(this.getMessage('E102'));
                    return false;
                }
                else {
                    $ctrl.showError(this.getMessage('E101'));
                    return false;
                }
            }
            if (isRequiredNotAllowZero && Number(inputValue) === 0) {
                $ctrl.showError(this.getMessage('E101'));
                return false;
            }
        }
        if (isrequirednullorempty1) {
            if (!inputValue) {
                $ctrl.showError(this.getMessage('E310'));
                return false;
            }
        }
        if (isrequirednullorempty2) {
            if (!inputValue) {
                $ctrl.showError(this.getMessage('E312'));
                return false;
            }
        }
        if (isrequirednullorempty3) {
            if (!inputValue) {
                $ctrl.showError(this.getMessage('E205'));
                return false;
            }
        }
        if (inputValue) {
            if (isSingleDoubleByte) {
                const maxLength = $ctrl.attr('maxlength');
                if (maxLength) {
                    const byteLength = this.getStringByteCount(inputValue);
                    if (byteLength > parseInt(maxLength)) {
                        $ctrl.showError(this.getMessage('E105'));
                        return;
                    }
                }
            }

            if (isDoubleByte) {
                const regex = new RegExp(regexPattern.doublebyte);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E107'));
                    return;
                }

                const maxLength = $ctrl.attr('maxlength');
                if (maxLength) {
                    const byteLength = this.getStringByteCount(inputValue);
                    if (byteLength > parseInt(maxLength)) {
                        $ctrl.showError(this.getMessage('E105'));
                        return;
                    }
                }
            }

            if (isSingleByte) {
                const regex = new RegExp(regexPattern.singlebyte);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }
            }

            if (isSingleByteNumber) {
                const regex = new RegExp(regexPattern.singlebyte_number);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }
            }

            if (isSingleByteNumberAlpha) {
                const regex = new RegExp(regexPattern.singlebyte_numberAlpha);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }
            }

            if (isNumeric) {
                const regex = new RegExp(regexPattern.numeric);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }

                const intdigits = parseInt($ctrl.data('integerdigits'));
                const decdigits = parseInt($ctrl.data('decimaldigits'));
                const parts = inputValue.split('.');
                let intpart = parts[0];
                let decpart = parts.length > 1 ? parts[1] : "0";
                if (intpart.length > intdigits) {
                    $ctrl.showError(this.getMessage('E105'));
                    return;
                }

                if (decdigits > 0) {
                    if (decpart.length > decdigits) {
                        decpart = decpart.substr(0, decdigits);
                    }
                    else {
                        decpart = ('0'.repeat(decdigits) + decpart).slice(-1 * decdigits);
                    }
                    inputValue = intpart + '.' + decpart;
                    $ctrl.val(inputValue);
                }
                else {
                    inputValue = intpart;
                    $ctrl.val(intpart);
                }
            }

            if (isMoney) {
                const regex = new RegExp(regexPattern.moeney);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }

                const digits = $ctrl.data('digits');
                inputValue = inputValue.replace(/,/g, '');
                if (inputValue.length > parseInt(digits)) {
                    $ctrl.showError(this.getMessage('E105'));
                    return;
                }
                $ctrl.val(Number(inputValue).toLocaleString());
            }


            if (isOneByteCharacter) {
                const strByte = this.getStringByteCount(inputValue);
                if (strByte != inputValue.length) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }
            }


            if (isMaxlengthCheck) {
                const maxLength = $ctrl.data('digits');
                if (maxLength) {
                    const byteLength = this.getStringByteCount(inputValue);
                    if (byteLength > parseInt(maxLength)) {
                        $ctrl.showError(this.getMessage('E105'));
                        return;
                    }
                }
            }

            if (isMaxlengthCheckforsellerlist) {
                const maxLength = $ctrl.data('digits');
                if (maxLength) {
                    const byteLength = inputValue.length;
                    if (byteLength > parseInt(maxLength)) {
                        $ctrl.showError(this.getMessage('E105'));
                        return;
                    }
                }
            }

            if (isDate) {
                if (!inputValue.match(regexPattern.dateformat)) {
                    $ctrl.showError(this.getMessage('E108'));
                    return;
                }
            }

            if (isDateCompare) {
                if (!common.compareDate($("#StartDate").val(), $("#EndDate").val())) {
                    $ctrl.showError(this.getMessage('E111'));
                    return;
                }
            }
            if (ischeckboxLenght) {                
                if (!common.checkboxlengthCheck($ctrl.attr('class'))) {
                    $ctrl.showError(this.getMessage('E112'));
                    return;
                }
            }
        }

        if (customValidation) {
            func = new Function('arg1', 'return ' + customValidation + '(arg1)')
            if (!func($ctrl)) {
                return false;
            }
        }
        if (type === 'radio')
            $('input[name="' + name + '"]:radio').hideError();
        else
            $ctrl.hideError();

        if (type === 'checkbox')
            $('input[name="' + name + '"]:checkbox').hideError();
        else
            $ctrl.hideError();

        return true;
    },
    
    checkValidityOnSave: function checkValidityOnSave(selector) {
        let success = true;
        $(selector + ' :input:not(button):not(:hidden):not(:disabled):not([readonly])').each(function () {
            if (!common.checkValidityInput(this)) {
                success = false;
            }
        });
        return success;
    },
    
    bindValidationEvent: function bindValidationEvent(selector, exceptSelector) {
        var selector = selector + ' :input:not(button):not(:hidden):not(:disabled):not([readonly])';
        if (exceptSelector) {
            selector = selector + exceptSelector;
        }
        $(document).on('blur', selector, function (e) {
            return common.checkValidityInput(this);
        });
    },
   
}