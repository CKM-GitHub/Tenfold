window.addEventListener('pageshow', function (event) {
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
    E113: '値の大小が不正です',
    E125: '正しい年月を入力してください',
    E126: '年月の大小が不正です',

    E201: '査定サービスの対象外地域です',
    E202: 'メールアドレスが入力されていません',
    E203: '既に登録済のメールアドレスです',
    E204: 'メールアドレスを正しく入力してください',
    E205: 'パスワードが入力されていません',
    E206: 'メールアドレスとパスワードの組合せが正しくありません',
    E207: 'この情報での登録はありません',

    E303: '検索条件は必ずひとつ以上指定してください',
    E304: '画像ファイルを指定してください',
    E310: '会社IDが入力されていません',
    E311: '会社IDが未登録です',
    E312: 'スタッフIDが入力されていません',
    E313: 'スタッフIDが未入力です。',
    E314: '入力されたスタッフIDは登録済です',
    E315: '変更内容を保存しますか？',
    E316: '変更内容を保存しました',
    E317: '変更内容を取消しますか？',
    E318: '更新対象項目がありませんでした',
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
    monthformat: /^\d{4}-\d{2}$/,
    dateformat: /^\d{4}-\d{2}-\d{2}$/,
    doublebyteKana: "^[ァ-ヶー　]+$",
    singlebyte_number_minus:"^[0-9-]+$",
}

const kanaMap = {
    'ｶﾞ': 'ガ', 'ｷﾞ': 'ギ', 'ｸﾞ': 'グ', 'ｹﾞ': 'ゲ', 'ｺﾞ': 'ゴ',
    'ｻﾞ': 'ザ', 'ｼﾞ': 'ジ', 'ｽﾞ': 'ズ', 'ｾﾞ': 'ゼ', 'ｿﾞ': 'ゾ',
    'ﾀﾞ': 'ダ', 'ﾁﾞ': 'ヂ', 'ﾂﾞ': 'ヅ', 'ﾃﾞ': 'デ', 'ﾄﾞ': 'ド',
    'ﾊﾞ': 'バ', 'ﾋﾞ': 'ビ', 'ﾌﾞ': 'ブ', 'ﾍﾞ': 'ベ', 'ﾎﾞ': 'ボ',
    'ﾊﾟ': 'パ', 'ﾋﾟ': 'ピ', 'ﾌﾟ': 'プ', 'ﾍﾟ': 'ペ', 'ﾎﾟ': 'ポ',
    'ｳﾞ': 'ヴ', 'ﾜﾞ': 'ヷ', 'ｦﾞ': 'ヺ',
    'ｱ': 'ア', 'ｲ': 'イ', 'ｳ': 'ウ', 'ｴ': 'エ', 'ｵ': 'オ',
    'ｶ': 'カ', 'ｷ': 'キ', 'ｸ': 'ク', 'ｹ': 'ケ', 'ｺ': 'コ',
    'ｻ': 'サ', 'ｼ': 'シ', 'ｽ': 'ス', 'ｾ': 'セ', 'ｿ': 'ソ',
    'ﾀ': 'タ', 'ﾁ': 'チ', 'ﾂ': 'ツ', 'ﾃ': 'テ', 'ﾄ': 'ト',
    'ﾅ': 'ナ', 'ﾆ': 'ニ', 'ﾇ': 'ヌ', 'ﾈ': 'ネ', 'ﾉ': 'ノ',
    'ﾊ': 'ハ', 'ﾋ': 'ヒ', 'ﾌ': 'フ', 'ﾍ': 'ヘ', 'ﾎ': 'ホ',
    'ﾏ': 'マ', 'ﾐ': 'ミ', 'ﾑ': 'ム', 'ﾒ': 'メ', 'ﾓ': 'モ',
    'ﾔ': 'ヤ', 'ﾕ': 'ユ', 'ﾖ': 'ヨ',
    'ﾗ': 'ラ', 'ﾘ': 'リ', 'ﾙ': 'ル', 'ﾚ': 'レ', 'ﾛ': 'ロ',
    'ﾜ': 'ワ', 'ｦ': 'ヲ', 'ﾝ': 'ン',
    'ｧ': 'ァ', 'ｨ': 'ィ', 'ｩ': 'ゥ', 'ｪ': 'ェ', 'ｫ': 'ォ',
    'ｯ': 'ッ', 'ｬ': 'ャ', 'ｭ': 'ュ', 'ｮ': 'ョ',
    '｡': '。', '､': '、', 'ｰ': 'ー', '｢': '「', '｣': '」', '･': '・'
};
$(function () {
    $("form").bind("keypress", function (e) {
        if (e.keyCode == 13) {
            if (document.activeElement.type == 'password' || document.activeElement.id == 'btnLogin' || document.activeElement.id == 'btnDisplay' || document.activeElement.id == 'btnProcess')
                return true;
            else return false;
        }
    });
    $('#sidebar-wrapper').bind("keypress", function (e) {
        if (e.keyCode == 13) {
            if (document.activeElement.id == 'btnDisplay' || document.activeElement.id == 'btnProcess')
                return true;
            else return false;
        }
    });
})
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

    showErrorPopup: function showErrorPopup(id) {
        $('#site-error-modal-title').text(id);
        $('#site-error-modal-message').text(this.getMessage(id));
        $('#site-error-modal').modal('show');
    },

    showConfirmPopup: function showConfirmPopup(id, callbackYes, callbackNo) {
        let msg = this.getMessage(id);
        if (!msg) {
            msg = id;
            id = '';
        }

        $('#site-confirmation-modal-title').text(id);
        $('#site-confirmation-modal-message').text(msg);
        $('#site-confirmation-modal').modal('show');

        if (callbackYes) {
            $('#site-confirmation-modal-yes').click(callbackYes);
        }
        if (callbackNo) {
            $('#site-confirmation-modal-no').click(callbackNo);
        }
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

    redirectErrorPage: function redirectErrorPage(status) {
        let url;
        if (status == 400) url = "/Error/BadRequest";
        if (status == 401) url = "/Error/Unauthorized";
        if (status == 403) url = "/Error/Forbidden";
        if (status == 404) url = "/Error/NotFound";
        //if (status == 500) url = "/Error/InternalServerError";

        if (url) location.href = common.appPath + url;
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
            common.redirectErrorPage(XMLHttpRequest.status);
            if (failCallback) {
                failCallback();
            }
        });
    },
    callAjaxWithLoadingSync: function callAjaxWithLoadingSync(url, model, disableSelector, successCallback, failCallback) {
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
            async: false,
            timeout: 30000,
        }).done(function (data) {
            common.hideLoading(disableSelector);
            if (successCallback) {
                successCallback(data);
            }
        }).fail(function (XMLHttpRequest, status, e) {
            common.hideLoading(disableSelector);
            common.redirectErrorPage(XMLHttpRequest.status);
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
            //dataType: "json",
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
            common.redirectErrorPage(XMLHttpRequest.status);
            if (failCallback) {
                failCallback();
            }
        });
    },
    callSubmit: function callSubmit(form, action, fd) {
        const input = document.createElement('input');
        input.setAttribute('type', 'hidden');
        input.setAttribute('name', 'RequestVerificationToken');
        input.setAttribute('value', $("#_RequestVerificationToken").val());
        form.appendChild(input);

        form.method = "POST";
        form.action = action;
        if (fd) {
            form.addEventListener("formdata", eve => {
                for (const [name, value] of fd.entries()) {
                    eve.formData.append(name, value)
                }
            })
        }
        form.submit();
        this.showLoading();
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

    getFullWithOneCharactertwoByteCount: function mbStrWidth(input) {
        let len = 0;
        for (let i = 0; i < input.length; i++) {
            let code = input.charCodeAt(i);
            if ((code >= 0x0020 && code <= 0x1FFF) || (code >= 0xFF61 && code <= 0xFF9F)) {
                len += 1;
            } else if ((code >= 0x2000 && code <= 0xFF60) || (code >= 0xFFA0)) {
                len += 2;
            } else {
                len += 0;
            }
        }
        return len;
    },

    replaceDoubleToSingle: function replaceDoubleToSingle(str) {
        //return str.replace(/[！-～]/g, function (s) {
        //    return String.fromCharCode(s.charCodeAt(0) - 0xFEE0);
        //});
        let returnValue = str.replace(/[！-～]/g, function (s) {
            return String.fromCharCode(s.charCodeAt(0) - 0xFEE0);
        });
        return returnValue.replace(/　/g, " ");
    },

    replaceSingleToDouble: function replaceSingleToDouble(str) {
        //return str.replace(/[!-~]/g, function (s) {
        //    return String.fromCharCode(s.charCodeAt(0) + 0xFEE0);
        //});
        let returnValue = str.replace(/[!-~]/g, function (s) {
            return String.fromCharCode(s.charCodeAt(0) + 0xFEE0);
        });
        return returnValue.replace(/ /g, "　");
    },

    replaceSingleToDoubleKana: function replaceSingleToDoubleKana(str) {
        var reg = new RegExp('(' + Object.keys(kanaMap).join('|') + ')', 'g');
        return str
            .replace(reg, function (match) {
                return kanaMap[match];
            })
            .replace(/ﾞ/g, '゛')
            .replace(/ﾟ/g, '゜');
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
        const isDoubleByteKana = $ctrl.attr("data-validation-doublebyte-kana");
        const isSingleByte = $ctrl.attr("data-validation-singlebyte");
        const isSingleByteNumber = $ctrl.attr("data-validation-singlebyte-number");
        const isSingleByteNumberAlpha = $ctrl.attr("data-validation-singlebyte-numberalpha");
        const isNumeric = $ctrl.attr("data-validation-numeric");
        const isMoney = $ctrl.attr("data-validation-money");
        const customValidation = $ctrl.attr("data-validation-custom");
        const isDate = $ctrl.attr("data-validation-datecheck");
        const isMaxlengthCheck = $ctrl.attr("data-validation-maxlengthCheck");
        const isDateCompare = $ctrl.attr("data-validation-datecompare");
        const isOneByteCharacter = $ctrl.attr("data-validation-onebyte-character");
        const ischeckboxLenght = $ctrl.attr("data-validation-checkboxlenght");
        const ispasswordcompare = $ctrl.attr("data-validation-passwordcompare");
        const isMinlengthCheck = $ctrl.attr("data-validation-minlengthcheck");
        const isemailformat = $ctrl.attr("data-validation-email");
        const isAllowMinus = $ctrl.attr("data-allowminus");

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

        if (isDoubleByte || isDoubleByteKana) {
            inputValue = this.replaceSingleToDouble(inputValue);
            inputValue = this.replaceSingleToDoubleKana(inputValue);
            $ctrl.val(inputValue);
        }

        //validation
        if (!isRequired
            && !isSingleDoubleByte
            && !isDoubleByte && !isDoubleByteKana
            && !isSingleByte && !isSingleByteNumber && !isSingleByteNumberAlpha
            && !isNumeric && !isMoney && !isDate && !isMaxlengthCheck && !isOneByteCharacter && !ischeckboxLenght
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
                    const byteLength = this.getFullWithOneCharactertwoByteCount(inputValue);
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
            }

            if (isDoubleByteKana) {
                const regex = new RegExp(regexPattern.doublebyteKana);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
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
                if ($ctrl.prop("id") == "ContactPhone") {
                    const regex_number_minus = new RegExp(regexPattern.singlebyte_number_minus);
                    if (!regex_number_minus.test(inputValue)) {
                        $ctrl.showError(this.getMessage('E104'));
                        return;
                    }
                }
                else {
                    const regex = new RegExp(regexPattern.singlebyte_number);
                    if (!regex.test(inputValue)) {
                        $ctrl.showError(this.getMessage('E104'));
                        return;
                    }
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
                let isMinus = false;
                if (isAllowMinus) {
                    if (inputValue.substr(0, 1) === '-') {
                        inputValue = inputValue.substr(1);
                        isMinus = true;
                    } else if (inputValue.substr(-1) === '-') {
                        inputValue = inputValue.slice(0, -1);
                        isMinus = true;
                    }
                }

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
                        //decpart = ('0'.repeat(decdigits) + decpart).slice(-1 * decdigits);
                        decpart = (decpart + '0'.repeat(decdigits)).slice(0, decdigits);
                    }
                    inputValue = intpart + '.' + decpart;
                    if (isMinus) inputValue = '-' + inputValue;
                    $ctrl.val(inputValue);
                }
                else {
                    inputValue = intpart;
                    if (isMinus) inputValue = '-' + inputValue;
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
                    const byteLength = inputValue.length;
                    if (byteLength > parseInt(maxLength)) {
                        $ctrl.showError(this.getMessage('E105'));
                        return;
                    }
                }
            }

            if (isMinlengthCheck) {
                const minLength = $ctrl.data('mindigits');
                const byteLength = inputValue.length;
                if (byteLength < parseInt(minLength)) {
                        $ctrl.showError(this.getMessage('E110'));
                        return;
                }
            }


            if (isDate) {
                if ($ctrl.attr('type') == 'date') {
                    if (!inputValue.match(regexPattern.dateformat)) {
                        $ctrl.showError(this.getMessage('E108'));
                        return;
                    }
                }
                else if ($ctrl.attr('type') == 'month' && ($("#StartMonth").val() != "" || $("#EndMonth").val() != ""))
                {
                    if (!inputValue.match(regexPattern.monthformat)) {
                        $ctrl.showError(this.getMessage('E125'));
                        return;
                    }
                }
                else {
                    if (!inputValue.match(regexPattern.monthformat)) {
                        $ctrl.showError(this.getMessage('E108'));
                        return;
                    }
                }
            }

            if (isDateCompare) {
                if ($("#StartDate").val() != undefined && $("#EndDate").val() != undefined) {
                    if (!common.compareDate($ctrl.attr('type'), $("#StartDate").val(), $("#EndDate").val())) {
                        $("#StartDate").showError(this.getMessage('E111'));
                        $("#EndDate").showError(this.getMessage('E111'));
                        $("#StartDate").focus();
                        return;
                    }
                }

                if ($("#StartYear").val() != "" && $("#EndYear").val() != "") {
                    if (!common.compareYear($("#StartYear").val(), $("#EndYear").val())) {
                        $("#StartYear").showError(this.getMessage('E113'));
                        $("#StartYear").focus();
                        return;
                    }
                }

                if ($("#StartMonth").val() != undefined && $("#EndMonth").val() != undefined) {
                    if (!common.compareMonth($("#StartMonth").val(), $("#EndMonth").val())) {
                        $("#StartMonth").showError(this.getMessage('E126'));
                        $("#EndMonth").showError(this.getMessage('E126'));
                        $("#StartMonth").focus();
                        return;
                    }
                }

            }

            if (ischeckboxLenght) {                
                if (!common.checkboxlengthCheck($ctrl.attr('class'))) {
                    $ctrl.showError(this.getMessage('E112'));
                    return;
                }
            }

            if (ispasswordcompare) {
                if (!common.comparePassword($("#txtPassword").val(), $("#txtConfirmPassword").val())) {
                    $("#txtPassword").showError(this.getMessage('E109'));
                    $("#txtConfirmPassword").showError(this.getMessage('E109'));
                    $("#txtPassword").focus();
                    return;
                }
            }
            if (isemailformat) {
                if (!inputValue.match(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)) {
                    $ctrl.showError(this.getMessage('E204'));
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

    compareDate: function compareTwoDate(type, d1, d2) {
        if (type == 'month') {
            d1 = d1 + '-01';
            d2 = d2 + '-' + new Date(parseInt(d2.slice(0, 4)), parseInt(d2.slice(5, 7)), 0).getDate();
        }
        const date1 = new Date(d1);
        const date2 = new Date(d2);
        let success = true;
        if (date1 > date2) {
            success = false;
        }
        return success;
    },

    compareMonth: function compareTwoMY(d1, d2) {
        const date1 = new Date(d1);
        const date2 = new Date(d2);
        let success = true;
        if (date1 > date2) {
            success = false;
        }
        return success;
    },

    comparePassword: function compareTwoPassword(pw, c_pw) {
        let success = true;
        if (pw !== c_pw) {
            success = false;
        }
        return success;
    },

    compareYear: function compareTwoDate(d1, d2) {
        const Year1 = Number(d1);
        const Year2 = Number(d2);
        let success = true;
        if (Year1 > Year2) {
            success = false;
        }
        return success;
    },

    checkboxlengthCheck: function checkboxlengthCheck(className) {
        if (className.includes(" "))
            className = className.split(" ")[0];
        let success = true;
        let checked = $("." + className + ":checked").length;
        if (!checked) {
            success = false;
        }
        return success;
    },

    getToday: function getToday() {
        let now = new Date();
        let day = ("0" + now.getDate()).slice(-2);
        let month = ("0" + (now.getMonth() + 1)).slice(-2);
        let today = now.getFullYear() + "-" + (month) + "-" + (day);
        return today;
    },
    getFirstDayofMonth: function getFirstDayofMonth() {
        let date = new Date();
        let firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        return firstDay.getFullYear() + "-" + ("0" + (firstDay.getMonth() + 1)).slice(-2) + "-" + ("0" + firstDay.getDate()).slice(-2);
    },
    getFirstDayofPreviousMonth: function getFirstDayofPreviousMonth() {
        let date = new Date();
        let fdaypremonth = new Date(date.getFullYear(), date.getMonth() - 1, 1);
        return fdaypremonth.getFullYear() + "-" + ("0" + (fdaypremonth.getMonth() + 1)).slice(-2) + "-" + ("0" + fdaypremonth.getDate()).slice(-2);
    },
    getLastDayofPreviousMonth: function getLastDayofPreviousMonth() {
        let date = new Date();
        let ldaypremonth = new Date(date.getFullYear(), date.getMonth(), 0);
        return ldaypremonth.getFullYear() + "-" + ("0" + (ldaypremonth.getMonth() + 1)).slice(-2) + "-" + ("0" + ldaypremonth.getDate()).slice(-2);
    },
    getDayaweekbeforetoday: function getDayaweekbeforetoday() {
        //var date = new Date();
        //date.setDate(date.getDate() - 7);
        //let data = date.toISOString().slice(0, 10);
        let today = new Date();
        let first = today.getDate() - today.getDay() + 1;

        let monday = new Date(today.setDate(first));
        let data = monday.toISOString().slice(0, 10);
        return data;
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

    getJSONtoCSV: function JSON2CSV(objArray) {
        if (!(objArray == "[]")) {
            var array = typeof JSONString != 'object' ? JSON.parse(objArray) : JSONString;
            var fields = Object.keys(array[0])
            var replacer = function (key, value) { return value === null ? null : value }
            var csv = array.map(function (row) {
                return fields.map(function (fieldName) {
                    return JSON.stringify(row[fieldName], replacer)
                }).join(',')
            })
            csv.unshift(fields.join(',')) // add header column
            csv = csv.join('\r\n');
            return csv;
        }
        else {
            return "ERROR";
        }
    },

    setValidationErrors: function setValidationErrors(errors) {
        for (key in errors) {
            const target = document.getElementById(key);
            $(target).showError(errors[key]);
        }
    },

    setFocusFirstError: function setFocusFirstError(form) {
        if (form) {
            const $target = $(form).getInvalidItems().get(0);
            if ($target) {
                $target.focus();
                return true;
            }
        }
        return false;
    },
   
}
