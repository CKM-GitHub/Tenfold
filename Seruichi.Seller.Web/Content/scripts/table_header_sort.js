const sortTable = {
    getSortingTable: function (selector) {
        
        //sorting all header
        const table = document.getElementById(selector);
        const headers = table.querySelectorAll('th');
        const tableBody = table.querySelector('tbody');
        const rows = tableBody.querySelectorAll('tr'); 
        // Track sort directions
        const directions = Array.from(headers).map(function (header) {
            return '';
        });

        // Transform the content of given cell in given column
        const transform = function (index, content) {
            // Get the data type of column           
            const type = headers[index].getAttribute('data-type');
            switch (type) {
                case 'number':
                    return parseFloat(content);
                case 'string':
                default:
                    return content;
            }
        };

        const sortColumn = function (index) {
           // debugger;
            var col_index = [];
            if (headers[index].hasAttribute('ordercol-index'))
                col_index = headers[index].getAttribute('ordercol-index').split('_');
            else
                col_index[0] = index;
            // Get the current direction
            const direction = directions[col_index[0]] || 'asc';

            // A factor based on the direction
            const multiplier = direction === 'asc' ? 1 : -1;

            const newRows = Array.from(rows);
           
            newRows.sort(function (rowA, rowB) {
                 
                var count = col_index.length;
                let cellA, cellB, cellC, cellD, cellE, cellF;
                let a, b, c, d, e, f;
                var index1 = col_index[0];
                if (rowA.querySelectorAll('td')[index1].querySelectorAll('a').length > 0) {
                    cellA = rowA.querySelectorAll('td')[index1].querySelectorAll('a')[0].innerHTML;
                    cellB = rowB.querySelectorAll('td')[index1].querySelectorAll('a')[0].innerHTML;
                }
                else {
                    cellA = rowA.querySelectorAll('td')[index1].innerHTML;
                    cellB = rowB.querySelectorAll('td')[index1].innerHTML;
                } 
                a = transform(index1, cellA);
                b = transform(index1, cellB);

                switch (count) {
                    case 1: if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                        else return b.localeCompare(a, "ja-JP");

                    case 2: var index2 = col_index[1];
                        if (rowA.querySelectorAll('td')[index2].querySelectorAll('a').length > 0) {
                            cellC = rowA.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellC = rowA.querySelectorAll('td')[index2].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].innerHTML;
                        }
                        c = transform(index2, cellC);
                        d = transform(index2, cellD);

                        if (a !== b) {
                            if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                            else return b.localeCompare(a, "ja-JP");
                        }
                        else if (c !== d) {
                            if (multiplier == 1) return c.localeCompare(d, "ja-JP");
                            else return d.localeCompare(c, "ja-JP");
                        }

                    case 3: var index2 = col_index[1];
                        if (rowA.querySelectorAll('td')[index2].querySelectorAll('a').length > 0) {
                            cellC = rowA.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellC = rowA.querySelectorAll('td')[index2].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].innerHTML;
                        }
                        c = transform(index2, cellC);
                        d = transform(index2, cellD);

                        var index3 = col_index[2];
                        if (rowA.querySelectorAll('td')[index3].querySelectorAll('a').length > 0) {
                            cellE = rowA.querySelectorAll('td')[index3].querySelectorAll('a')[0].innerHTML;
                            cellF = rowB.querySelectorAll('td')[index3].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellE = rowA.querySelectorAll('td')[index3].innerHTML;
                            cellF = rowB.querySelectorAll('td')[index3].innerHTML;
                        }
                        e = transform(index3, cellE);
                        f = transform(index3, cellF);

                        if (a !== b) {
                            if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                            else return b.localeCompare(a, "ja-JP");
                        }
                        else if (c !== d) {
                            if (multiplier == 1) return c.localeCompare(d, "ja-JP");
                            else return d.localeCompare(c, "ja-JP");
                        }
                        else if (e !== f) {
                            if (multiplier == 1) return e.localeCompare(f, "ja-JP");
                            else return f.localeCompare(e, "ja-JP");
                        }
                }
            });

            // Remove old rows
            [].forEach.call(rows, function (row) {
                tableBody.removeChild(row);
            });

            // Reverse the direction
            directions[col_index[0]] = direction === 'asc' ? 'desc' : 'asc';

            // Append new row
            newRows.forEach(function (newRow) {
                tableBody.appendChild(newRow);
            });
        };

        [].forEach.call(headers, function (header, index) {
            header.addEventListener('click', function () {
                if ($(this).hasClass('sort-by')) {
                    sortColumn(index);
                    if($('#pager').children('li').eq(1)) 
                    $('#pager').children('li').eq(1).click();
                }
            });
        });
    }   
}
