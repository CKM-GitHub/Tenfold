
  $.fn.extend({
    treed: function (o) {
      
      var openedClass = 'glyphicon-minus-sign';
      var closedClass = 'glyphicon-plus-sign';
      
      if (typeof o != 'undefined'){
        if (typeof o.openedClass != 'undefined'){
        openedClass = o.openedClass;
        }
        if (typeof o.closedClass != 'undefined'){
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
      tree.find('.branch .indicator').each(function(){
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

$('#tree').treed({openedClass:'fa-minus', closedClass:'fa-plus'});


//checkbox-indeterminate
$(document).on('change','input[type="checkbox"]',function(e) {
  var checked = $(this).prop("checked");
  var container = $(this).parent();
  var siblings = container.siblings();
   
    
  container.find('input[type="checkbox"]').prop({
    indeterminate: false,
    checked: checked
  });

    function checkSiblings(el) {
      var parent = el.parent().parent();
     
       
      var all = true;

        el.siblings().each(function () {
            parent.children('a').attr("style", "color:red");
      return all = ($(this).children('input[type="checkbox"]').prop("checked") === checked);
    });

        if (all && checked) {
           
      parent.children('input[type="checkbox"]').prop({
        indeterminate: false,
        checked: checked
      });

        parent.children('a').attr("style", "color:red");
       // checkSiblings(parent);
        

        } else if (all && !checked) {
           
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
            el.parents("li").parents().children("a").attr("style", "color:red");
       
          
        
    }

  }

  checkSiblings(container);
});