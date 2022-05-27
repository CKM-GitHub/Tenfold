(function ($) {

    $.fn.hideChildErrors = function () {
        const $this = this;
        $this.find('.cap-is-invalid').each(function () {
            $(this).hideError();
        });
        return $this;
    };

    $.fn.isInvalid = function () {
        return this.hasClass('cap-is-invalid');
    };
    
    $.fn.getInvalidItems = function () {
        return this.find('.cap-is-invalid');
    };

    $.fn.showError = function (message) {
        const $this = this;
        $this.addClass('cap-is-invalid').attr('data-errormsg', message);
        $this.each(function () {
            const selector = $(this).data('errormsg-target');
            if (selector) {
                const othermsg = getFirstErrorMsg(selector);
                if (othermsg)
                    $(selector).text(othermsg).css('display', 'block');
                else
                    $(selector).text(message).css('display', 'block');
            }
        });
        return $this;
    };

    $.fn.hideError = function () {
        const $this = this;
        $this.removeClass('cap-is-invalid').removeAttr('data-errormsg');
        $this.each(function () {
            const selector = $(this).data('errormsg-target');
            if (selector) {
                const othermsg = getFirstErrorMsg(selector);
                if (othermsg)
                    $(selector).text(othermsg);
                else
                    $(selector).text('').css('display', 'none');
            }
        });
        return $this;
    };

    $.fn.addvalidation_reqired = function (notAllowZero) {
    //notAllowZero: zero is error
        this.attr('data-validation-required', 'true');
        notAllowZero && this.attr('data-validation-not-allow-zero', 'true');
        return this;
    };
    $.fn.removeValidation_required = function () {
        return this
            .removeAttr('data-validation-required')
            .removeAttr('data-validation-not-allow-zero');
    };


    $.fn.addvalidation_singlebyte_doublebyte = function () {
        return this
            .attr('data-validation-singlebyte-doublebyte', 'true')
            .attr('inputmode', 'kana');
    };
    $.fn.removeValidation_singlebyte_doublebyte = function () {
        return this
            .removeAttr('data-validation-singlebyte-doublebyte')
    };


    $.fn.addvalidation_doublebyte = function () {
        return this
            .attr('data-validation-doublebyte', 'true')
            .attr('inputmode', 'kana');
    };
    $.fn.removeValidation_doublebyte = function () {
        return this
            .removeAttr('data-validation-doublebyte');
    };

    $.fn.addvalidation_doublebyte_kana = function () {
        return this
            .attr('data-validation-doublebyte-kana', 'true')
            .attr('inputmode', 'kana');
    };
    $.fn.removeValidation_doublebyte_kana = function () {
        return this
            .removeAttr('data-validation-doublebyte-kana');
    };

    $.fn.addvalidation_doublebyte_hira = function () {
        return this
            .attr('data-validation-doublebyte-hira', 'true')
            .attr('inputmode', 'hira');
    };
    $.fn.removeValidation_doublebyte_hira = function () {
        return this
            .removeAttr('data-validation-doublebyte-hira');
    };

    $.fn.addvalidation_singlebyte = function () {
        return this
            .attr('data-validation-singlebyte', 'true')
            .attr('inputmode', 'text');
    };
    $.fn.removeValidation_singlebyte = function () {
        return this
            .removeAttr('data-validation-singlebyte');
    };


    $.fn.addvalidation_singlebyte_number = function () {
        return this
            .attr('data-validation-singlebyte-number', 'true')
            .attr('inputmode', 'numeric');
    };
    $.fn.removeValidation_singlebyte_number = function () {
        return this.removeAttr('data-validation-singlebyte-number')
    };

    $.fn.addvalidation_singlebyte_numberAlphabet = function () {
        this.attr('data-validation-singlebyte-numberalpha', 'true')
            .attr('inputmode', 'text');
    };
    $.fn.removeValidation_singlebyte_numberAlphabet = function () {
        return this.removeAttr('data-validation-singlebyte-numberalpha')
    };

    $.fn.addvalidation_singlebyte_kana = function () {
        return this
            .attr('data-validation-singlebyte-kana', 'true')
            .attr('inputmode', 'kana');
    };
    $.fn.removevalidation_singlebyte_kana = function () {
        return this
            .removeAttr('data-validation-singlebyte-kana');
    };
    $.fn.addvalidation_numeric = function (integerdigits, decimaldigits) {
        return this
            .attr('data-validation-numeric', 'true')
            .attr('data-integerdigits', integerdigits)
            .attr('data-decimaldigits', decimaldigits)
            .attr('inputmode', 'decimal')
            .attr('autocomplete', 'off');
    };
    $.fn.removeValidation_numeric = function () {
        return this
            .removeAttr('data-validation-numeric')
            .removeAttr('data-integerdigits')
            .removeAttr('data-decimaldigits');
    };


    $.fn.addvalidation_money = function (digits) {
        return this
            .attr('data-validation-money', 'true')
            .attr('data-digits', digits)
            .attr('inputmode', 'decimal')
            .attr('autocomplete', 'off');
    };
    $.fn.removeValidation_money = function () {
        return this
            .removeAttr('data-validation-money')
            .removeAttr('data-digits')
    };

    //add by pnz
    $.fn.addvalidation_maxlengthCheck = function (digits) {
        return this
            .attr('add-validation-maxlengthCheck', 'true')
            .attr('data-digits', digits)
            .attr('inputmode', 'text')
            .attr('autocomplete', 'off');
    };

    $.fn.addvalidation_onebyte_character = function () {
        return this
            .attr('data-validation-onebyte-character', 'true')
            .attr('inputmode', 'text');
    };

    //--validation
    $.fn.addvalidation_datecheck = function () {
        return this
            .attr('data-validation-datecheck', 'true')
    };
    $.fn.removeValidation_datecheck = function () {
        return this.removeAttr('data-validation-datecheck')
    };

    $.fn.addvalidation_datecompare = function () {
        return this
            .attr('data-validation-datecompare', 'true')
    };
    $.fn.removeValidation_datecompare = function () {
        return this.removeAttr('data-validation-datecompare')
    };

    $.fn.addvalidation_numcompare = function () {
        return this
            .attr('data_validation_numcompare', 'true')
    };
    $.fn.removeValidation_numcompare = function () {
        return this.removeAttr('data_validation_numcompare')
    };


    $.fn.addvalidation_checkboxlenght = function () {
        return this
            .attr('data-validation-checkboxlenght', 'true')
    };
    $.fn.removeValidation_checkboxlenght = function () {
        return this.removeAttr('data-validation-checkboxlenght')
    };    

    $.fn.addvalidation_minlengthCheck = function (digits) {
        return this
            .attr('data-validation-minlengthCheck', 'true')
            .attr('data-mindigits', digits)
            .attr('inputmode', 'text')
            .attr('autocomplete', 'off');
    };

    $.fn.addvalidation_passwordcompare = function () {
        return this
            .attr('data-validation-passwordcompare', 'true')
    };
    $.fn.removeValidation_passwordcompare = function () {
        return this.removeAttr('data-validation-passwordcompare')
    };

    $.fn.addvalidation_custom = function (validationName) {
        return this
            .attr('data-validation-custom', validationName);
    };
    $.fn.removeValidation_cusom = function () {
        return this
            .removeAttr('data-validation-custom');
    };


    $.fn.addvalidation_errorElement = function (errormsgElement) {
        return this.attr('data-errormsg-target', errormsgElement);
    };

    function getFirstErrorMsg(selector) {
        var group = $('.cap-is-invalid[data-errormsg-target="' + selector + '"]');
        if (group.length > 0) {
            return $(group.get(0)).attr('data-errormsg');
        }
        return "";
    }
})(jQuery);
