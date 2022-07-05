
$.fn.extend({
    treed: function (o) {

        var openedClass = 'glyphicon-minus-sign';
        var closedClass = 'glyphicon-plus-sign';

        if (typeof o != 'undefined') {
            if (typeof o.openedClass != 'undefined') {
                openedClass = o.openedClass;
            }
            if (typeof o.closedClass != 'undefined') {
                closedClass = o.closedClass;
            }
        };
        //initialize each of the top levels
        var tree = $(this);
        tree.addClass("tree");
        tree.find('li').has("ul").each(function () {
            var branch = $(this); //li with children ul
            branch.prepend("<i class='indicator fa " + closedClass + "'></i>");
            branch.addClass('branch');
            branch.on('click', function (e) {
                if (this == e.target) {
                    var icon = $(this).children('i:first');
                    icon.toggleClass(openedClass + " " + closedClass);
                    $(this).children().children().toggle();
                }
            })
            branch.children().children().toggle();
        });
        //fire event from the dynamically added icon
        tree.find('.branch .indicator').each(function () {
            $(this).on('click', function () {
                $(this).closest('li').click();
            });
        });
        //fire event to open branch if the li contains an anchor instead of text
        tree.find('.branch>a').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
        //fire event to open branch if the li contains a button instead of text
        tree.find('.branch>button').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
    }
});

$('#tree').treed({ openedClass: 'fa-minus', closedClass: 'fa-plus' });


//checkbox-indeterminate
$(document).on('change', '#tree input[type="checkbox"]', function (e) {
    var checked = $(this).prop("checked");
    var container = $(this).parent();
    var siblings = container.siblings();
    var city = $('#DisplayCityData').length > 0 ? $('#DisplayCityData').text() : '';

    container.find('input[type="checkbox"]').prop({
        indeterminate: false,
        checked: checked
    });

    container.find('input[type="checkbox"]').each(function () {
        if ($(this).hasClass('city')) {
            if ($(this).prop('checked')) {
                if (!city.includes($(this).next('label').text()))
                    city += city == '' ? $(this).next('label').text() : '/' + $(this).next('label').text();
            }
            else
                city = city.replace($(this).next('label').text(), '');
            city = city.replace('//', '/');

            if (city.substr(city.length - 1) == '/')
                city = city.substring(0, city.length - 1);

            if (city.charAt(0) == '/')
                city = city.substring(1, city.length);
        }
    });
    if ($('#DisplayCityData').length > 0) {
        $('#DisplayCityData').text(city);
    }

    function checkSiblings(el) {
        var parent = el.parent().parent();
        var all = true;

        el.siblings().each(function () {
            parent.children('a').attr("style", "color:" + "#0d6efd");
            return all = ($(this).children('input[type="checkbox"]').prop("checked") === checked);
        });

        if (all && checked) {
            parent.children('input[type="checkbox"]').prop({
                indeterminate: false,
                checked: checked
            });

            parent.children('a').attr("style", "color:" + "#0d6efd");
            // checkSiblings(parent);
        }
        else if (all && !checked) {
            parent.children('input[type="checkbox"]').prop("checked", checked);
            parent.children('input[type="checkbox"]').prop("indeterminate", (parent.find('input[type="checkbox"]:checked').length > 0));
            // checkSiblings(parent);

            parent.children('a').removeAttr("style");
            el.parents("li").parents().children("a").removeAttr("style");
        }
        else {
            el.parents("li").children('input[type="checkbox"]').prop({
                indeterminate: true,
                checked: false
            });
            el.parents("li").parents().children("a").attr("style", "color:" + "#0d6efd");
        }
    }

    checkSiblings(container);
});


//--------------------------------------------------
//Tree view over 3 levels
$('#tree_multilevel').treed({ openedClass: 'fa-minus', closedClass: 'fa-plus' });

$(document).on('change', '#tree_multilevel input[type="checkbox"]', function (e) {
    var checked = $(this).prop("checked");
    var container = $(this).parent();

    container.find('input[type="checkbox"]').prop({
        indeterminate: false,
        checked: checked
    });

    function checkSiblings(el) {
        var parent = el.parent().parent(); //li
        var all = (el.children('input[type="checkbox"]').prop("indeterminate") === false);
        var isTop = false;

        el.siblings().each(function () {
            const $checkbox = $(this).children('input[type="checkbox"]');
            return all = ($checkbox.prop("checked") === checked && $checkbox.prop("indeterminate") === false);
        });

        if (all && checked) {
            parent.children('input[type="checkbox"]').prop({
                indeterminate: false,
                checked: checked
            });

            isTop = parent.children('a').attr("style", "color:" + "#0d6efd").length > 0;
        }
        else if (all && !checked) {
            parent.children('input[type="checkbox"]').prop("checked", checked);
            parent.children('input[type="checkbox"]').prop("indeterminate", (parent.find('input[type="checkbox"]:checked').length > 0));

            isTop = parent.children('a').removeAttr("style").length > 0;
        }
        else {
            el.parents("li").children('input[type="checkbox"]').prop({
                indeterminate: true,
                checked: false
            });

            isTop = parent.children('a').attr("style", "color:" + "#0d6efd").length > 0;
        }

        return isTop;
    }

    while (checkSiblings(container) === false) {
        container = container.parent().parent();
    };
});
