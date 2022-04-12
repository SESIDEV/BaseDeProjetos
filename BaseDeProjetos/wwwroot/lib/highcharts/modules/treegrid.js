/*
 Highcharts Gantt JS v8.1.2 (2020-06-16)

 Tree Grid

 (c) 2016-2019 Jon Arild Nygard

 License: www.highcharts.com/license
*/
(function (a) { "object" === typeof module && module.exports ? (a["default"] = a, module.exports = a) : "function" === typeof define && define.amd ? define("highcharts/modules/treegrid", ["highcharts"], function (B) { a(B); a.Highcharts = B; return a }) : a("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (a) {
    function B(a, u, w, I) { a.hasOwnProperty(u) || (a[u] = I.apply(null, w)) } a = a ? a._modules : {}; B(a, "parts-gantt/Tree.js", [a["parts/Utilities.js"]], function (a) {
        var u = a.extend, w = a.isNumber, A = a.pick, g = function (a, n) {
            var m = a.reduce(function (f,
                m) { var a = A(m.parent, ""); "undefined" === typeof f[a] && (f[a] = []); f[a].push(m); return f }, {}); Object.keys(m).forEach(function (f, a) { var t = m[f]; "" !== f && -1 === n.indexOf(f) && (t.forEach(function (f) { a[""].push(f) }), delete a[f]) }); return m
        }, q = function (a, n, m, f, G, g) {
            var y = 0, k = 0, z = g && g.after, c = g && g.before; n = { data: f, depth: m - 1, id: a, level: m, parent: n }; var d, h; "function" === typeof c && c(n, g); c = (G[a] || []).map(function (c) {
                var b = q(c.id, a, m + 1, c, G, g), e = c.start; c = !0 === c.milestone ? e : c.end; d = !w(d) || e < d ? e : d; h = !w(h) || c > h ? c : h; y = y +
                    1 + b.descendants; k = Math.max(b.height + 1, k); return b
            }); f && (f.start = A(f.start, d), f.end = A(f.end, h)); u(n, { children: c, descendants: y, height: k }); "function" === typeof z && z(n, g); return n
        }; return { getListOfParents: g, getNode: q, getTree: function (a, n) { var m = a.map(function (f) { return f.id }); a = g(a, m); return q("", null, 1, null, a, n) } }
    }); B(a, "parts-gantt/TreeGridTick.js", [a["parts/Utilities.js"]], function (a) {
        var u = a.addEvent, w = a.defined, A = a.isObject, g = a.isNumber, q = a.pick, t = a.wrap, n; (function (a) {
            function f() {
                this.treeGrid ||
                (this.treeGrid = new z(this))
            } function m(c, d) {
                c = c.treeGrid; var h = !c.labelIcon, p = d.renderer, b = d.xy, e = d.options, l = e.width, D = e.height, E = b.x - l / 2 - e.padding; b = b.y - D / 2; var a = d.collapsed ? 90 : 180, f = d.show && g(b), v = c.labelIcon; v || (c.labelIcon = v = p.path(p.symbols[e.type](e.x, e.y, l, D)).addClass("highcharts-label-icon").add(d.group)); f || v.attr({ y: -9999 }); p.styledMode || v.attr({ "stroke-width": 1, fill: q(d.color, "#666666") }).css({ cursor: "pointer", stroke: e.lineColor, strokeWidth: e.lineWidth }); v[h ? "attr" : "animate"]({
                    translateX: E,
                    translateY: b, rotation: a
                })
            } function n(c, d, h, p, b, e, l, D, E) { var a = q(this.options && this.options.labels, e); e = this.pos; var f = this.axis, v = "treegrid" === f.options.type; c = c.apply(this, [d, h, p, b, a, l, D, E]); v && (d = a && A(a.symbol, !0) ? a.symbol : {}, a = a && g(a.indentation) ? a.indentation : 0, e = (e = (f = f.treeGrid.mapOfPosToGridNode) && f[e]) && e.depth || 1, c.x += d.width + 2 * d.padding + (e - 1) * a); return c } function y(c) {
                var d = this, h = d.pos, a = d.axis, b = d.label, e = a.treeGrid.mapOfPosToGridNode, l = a.options, D = q(d.options && d.options.labels, l && l.labels),
                E = D && A(D.symbol, !0) ? D.symbol : {}, f = (e = e && e[h]) && e.depth; l = "treegrid" === l.type; var k = -1 < a.tickPositions.indexOf(h); h = a.chart.styledMode; l && e && b && b.element && b.addClass("highcharts-treegrid-node-level-" + f); c.apply(d, Array.prototype.slice.call(arguments, 1)); l && b && b.element && e && e.descendants && 0 < e.descendants && (a = a.treeGrid.isCollapsed(e), m(d, { color: !h && b.styles && b.styles.color || "", collapsed: a, group: b.parentGroup, options: E, renderer: b.renderer, show: k, xy: b.xy }), E = "highcharts-treegrid-node-" + (a ? "expanded" :
                    "collapsed"), b.addClass("highcharts-treegrid-node-" + (a ? "collapsed" : "expanded")).removeClass(E), h || b.css({ cursor: "pointer" }), [b, d.treeGrid.labelIcon].forEach(function (e) {
                        e && !e.attachedTreeGridEvents && (u(e.element, "mouseover", function () { b.addClass("highcharts-treegrid-node-active"); b.renderer.styledMode || b.css({ textDecoration: "underline" }) }), u(e.element, "mouseout", function () { var e = w(D.style) ? D.style : {}; b.removeClass("highcharts-treegrid-node-active"); b.renderer.styledMode || b.css({ textDecoration: e.textDecoration }) }),
                            u(e.element, "click", function () { d.treeGrid.toggleCollapse() }), e.attachedTreeGridEvents = !0)
                    }))
            } var k = !1; a.compose = function (c) { k || (u(c, "init", f), t(c.prototype, "getLabelPosition", n), t(c.prototype, "renderLabel", y), c.prototype.collapse = function (d) { this.treeGrid.collapse(d) }, c.prototype.expand = function (d) { this.treeGrid.expand(d) }, c.prototype.toggleCollapse = function (d) { this.treeGrid.toggleCollapse(d) }, k = !0) }; var z = function () {
                function c(d) { this.tick = d } c.prototype.collapse = function (d) {
                    var c = this.tick, a = c.axis,
                    b = a.brokenAxis; b && a.treeGrid.mapOfPosToGridNode && (c = a.treeGrid.collapse(a.treeGrid.mapOfPosToGridNode[c.pos]), b.setBreaks(c, q(d, !0)))
                }; c.prototype.expand = function (d) { var c = this.tick, a = c.axis, b = a.brokenAxis; b && a.treeGrid.mapOfPosToGridNode && (c = a.treeGrid.expand(a.treeGrid.mapOfPosToGridNode[c.pos]), b.setBreaks(c, q(d, !0))) }; c.prototype.toggleCollapse = function (d) {
                    var c = this.tick, a = c.axis, b = a.brokenAxis; b && a.treeGrid.mapOfPosToGridNode && (c = a.treeGrid.toggleCollapse(a.treeGrid.mapOfPosToGridNode[c.pos]),
                        b.setBreaks(c, q(d, !0)))
                }; return c
            }(); a.Additions = z
        })(n || (n = {})); return n
    }); B(a, "mixins/tree-series.js", [a["parts/Color.js"], a["parts/Utilities.js"]], function (a, u) {
        var w = u.extend, A = u.isArray, g = u.isNumber, q = u.isObject, t = u.merge, n = u.pick; return {
            getColor: function (m, f) {
                var g = f.index, q = f.mapOptionsToLevel, y = f.parentColor, k = f.parentColorIndex, z = f.series, c = f.colors, d = f.siblings, h = z.points, p = z.chart.options.chart, b; if (m) {
                    h = h[m.i]; m = q[m.level] || {}; if (q = h && m.colorByPoint) {
                        var e = h.index % (c ? c.length : p.colorCount);
                        var l = c && c[e]
                    } if (!z.chart.styledMode) { c = h && h.options.color; p = m && m.color; if (b = y) b = (b = m && m.colorVariation) && "brightness" === b.key ? a.parse(y).brighten(g / d * b.to).get() : y; b = n(c, p, l, b, z.color) } var D = n(h && h.options.colorIndex, m && m.colorIndex, e, k, f.colorIndex)
                } return { color: b, colorIndex: D }
            }, getLevelOptions: function (a) {
                var f = null; if (q(a)) {
                    f = {}; var n = g(a.from) ? a.from : 1; var m = a.levels; var y = {}; var k = q(a.defaults) ? a.defaults : {}; A(m) && (y = m.reduce(function (a, c) {
                        if (q(c) && g(c.level)) {
                            var d = t({}, c); var f = "boolean" ===
                                typeof d.levelIsConstant ? d.levelIsConstant : k.levelIsConstant; delete d.levelIsConstant; delete d.level; c = c.level + (f ? 0 : n - 1); q(a[c]) ? w(a[c], d) : a[c] = d
                        } return a
                    }, {})); m = g(a.to) ? a.to : 1; for (a = 0; a <= m; a++)f[a] = t({}, k, q(y[a]) ? y[a] : {})
                } return f
            }, setTreeValues: function C(a, g) {
                var f = g.before, k = g.idRoot, z = g.mapIdToNode[k], c = g.points[a.i], d = c && c.options || {}, h = 0, p = []; w(a, {
                    levelDynamic: a.level - (("boolean" === typeof g.levelIsConstant ? g.levelIsConstant : 1) ? 0 : z.level), name: n(c && c.name, ""), visible: k === a.id || ("boolean" ===
                        typeof g.visible ? g.visible : !1)
                }); "function" === typeof f && (a = f(a, g)); a.children.forEach(function (b, e) { var l = w({}, g); w(l, { index: e, siblings: a.children.length, visible: a.visible }); b = C(b, l); p.push(b); b.visible && (h += b.val) }); a.visible = 0 < h || a.visible; f = n(d.value, h); w(a, { children: p, childrenTotal: h, isLeaf: a.visible && !h, val: f }); return a
            }, updateRootId: function (a) { if (q(a)) { var f = q(a.options) ? a.options : {}; f = n(a.rootNode, f.rootId, ""); q(a.userOptions) && (a.userOptions.rootId = f); a.rootNode = f } return f }
        }
    }); B(a, "parts-gantt/GridAxis.js",
        [a["parts/Axis.js"], a["parts/Globals.js"], a["parts/Options.js"], a["parts/Tick.js"], a["parts/Utilities.js"]], function (a, u, w, B, g) {
            var q = w.dateFormat, t = g.addEvent, n = g.defined, m = g.erase, f = g.find, A = g.isArray, C = g.isNumber, y = g.merge, k = g.pick, z = g.timeUnits, c = g.wrap; w = u.Chart; var d = function (b) { var e = b.options; e.labels || (e.labels = {}); e.labels.align = k(e.labels.align, "center"); b.categories || (e.showLastLabel = !1); b.labelRotation = 0; e.labels.rotation = 0 }; ""; a.prototype.getMaxLabelDimensions = function (b, e) {
                var a = {
                    width: 0,
                    height: 0
                }; e.forEach(function (e) { e = b[e]; if (g.isObject(e, !0)) { var l = g.isObject(e.label, !0) ? e.label : {}; e = l.getBBox ? l.getBBox().height : 0; l.textStr && !C(l.textPxLength) && (l.textPxLength = l.getBBox().width); l = C(l.textPxLength) ? Math.round(l.textPxLength) : 0; a.height = Math.max(e, a.height); a.width = Math.max(l, a.width) } }); return a
            }; u.dateFormats.W = function (a) {
                a = new this.Date(a); var e = (this.get("Day", a) + 6) % 7, b = new this.Date(a.valueOf()); this.set("Date", b, this.get("Date", a) - e + 3); e = new this.Date(this.get("FullYear",
                    b), 0, 1); 4 !== this.get("Day", e) && (this.set("Month", a, 0), this.set("Date", a, 1 + (11 - this.get("Day", e)) % 7)); return (1 + Math.floor((b.valueOf() - e.valueOf()) / 6048E5)).toString()
            }; u.dateFormats.E = function (a) { return q("%a", a, !0).charAt(0) }; t(w, "afterSetChartSize", function () { this.axes.forEach(function (a) { (a.grid && a.grid.columns || []).forEach(function (e) { e.setAxisSize(); e.setAxisTranslation() }) }) }); t(B, "afterGetLabelPosition", function (a) {
                var e = this.label, l = this.axis, b = l.reversed, d = l.chart, c = l.options.grid || {}, h =
                    l.options.labels, v = h.align, r = p.Side[l.side], x = a.tickmarkOffset, F = l.tickPositions, f = this.pos - x; F = C(F[a.index + 1]) ? F[a.index + 1] - x : l.max + x; var k = l.tickSize("tick"); x = k ? k[0] : 0; k = k ? k[1] / 2 : 0; if (!0 === c.enabled) {
                        if ("top" === r) { c = l.top + l.offset; var g = c - x } else "bottom" === r ? (g = d.chartHeight - l.bottom + l.offset, c = g + x) : (c = l.top + l.len - l.translate(b ? F : f), g = l.top + l.len - l.translate(b ? f : F)); "right" === r ? (r = d.chartWidth - l.right + l.offset, b = r + x) : "left" === r ? (b = l.left + l.offset, r = b - x) : (r = Math.round(l.left + l.translate(b ? F : f)) -
                            k, b = Math.round(l.left + l.translate(b ? f : F)) - k); this.slotWidth = b - r; a.pos.x = "left" === v ? r : "right" === v ? b : r + (b - r) / 2; a.pos.y = g + (c - g) / 2; d = d.renderer.fontMetrics(h.style.fontSize, e.element); e = e.getBBox().height; h.useHTML ? a.pos.y += d.b + -(e / 2) : (e = Math.round(e / d.h), a.pos.y += (d.b - (d.h - d.f)) / 2 + -((e - 1) * d.h / 2)); a.pos.x += l.horiz && h.x || 0
                    }
            }); var h = function () {
                function a(a) { this.axis = a } a.prototype.isOuterAxis = function () {
                    var a = this.axis, b = a.grid.columnIndex, d = a.linkedParent && a.linkedParent.grid.columns || a.grid.columns, c =
                        b ? a.linkedParent : a, h = -1, f = 0; a.chart[a.coll].forEach(function (e, b) { e.side !== a.side || e.options.isInternal || (f = b, e === c && (h = b)) }); return f === h && (C(b) ? d.length === b : !0)
                }; return a
            }(), p = function () {
                function b() { } b.compose = function (e) {
                    a.keepProps.push("grid"); c(e.prototype, "unsquish", b.wrapUnsquish); t(e, "init", b.onInit); t(e, "afterGetOffset", b.onAfterGetOffset); t(e, "afterGetTitlePosition", b.onAfterGetTitlePosition); t(e, "afterInit", b.onAfterInit); t(e, "afterRender", b.onAfterRender); t(e, "afterSetAxisTranslation",
                        b.onAfterSetAxisTranslation); t(e, "afterSetOptions", b.onAfterSetOptions); t(e, "afterSetOptions", b.onAfterSetOptions2); t(e, "afterSetScale", b.onAfterSetScale); t(e, "afterTickSize", b.onAfterTickSize); t(e, "trimTicks", b.onTrimTicks); t(e, "destroy", b.onDestroy)
                }; b.onAfterGetOffset = function () { var a = this.grid; (a && a.columns || []).forEach(function (a) { a.getOffset() }) }; b.onAfterGetTitlePosition = function (a) {
                    if (!0 === (this.options.grid || {}).enabled) {
                        var e = this.axisTitle, d = this.height, c = this.horiz, h = this.left, f = this.offset,
                        v = this.opposite, r = this.options.title, x = void 0 === r ? {} : r; r = this.top; var F = this.width, g = this.tickSize(), p = e && e.getBBox().width, z = x.x || 0, y = x.y || 0, m = k(x.margin, c ? 5 : 10); e = this.chart.renderer.fontMetrics(x.style && x.style.fontSize, e).f; g = (c ? r + d : h) + (c ? 1 : -1) * (v ? -1 : 1) * (g ? g[0] / 2 : 0) + (this.side === b.Side.bottom ? e : 0); a.titlePosition.x = c ? h - p / 2 - m + z : g + (v ? F : 0) + f + z; a.titlePosition.y = c ? g - (v ? d : 0) + (v ? e : -e) / 2 + f + y : r - m + y
                    }
                }; b.onAfterInit = function () {
                    var e = this.chart, b = this.options.grid; b = void 0 === b ? {} : b; var h = this.userOptions;
                    b.enabled && (d(this), c(this, "labelFormatter", function (a) { var e = this.axis, b = this.value, d = e.tickPositions, c = (e.isLinked ? e.linkedParent : e).series[0], l = b === d[0]; d = b === d[d.length - 1]; c = c && f(c.options.data, function (a) { return a[e.isXAxis ? "x" : "y"] === b }); this.isFirst = l; this.isLast = d; this.point = c; return a.call(this) })); if (b.columns) for (var k = this.grid.columns = [], g = this.grid.columnIndex = 0; ++g < b.columns.length;) {
                        var p = y(h, b.columns[b.columns.length - g - 1], { linkedTo: 0, type: "category" }); delete p.grid.columns; p = new a(this.chart,
                            p); p.grid.isColumn = !0; p.grid.columnIndex = g; m(e.axes, p); m(e[this.coll], p); k.push(p)
                    }
                }; b.onAfterRender = function () {
                    var a = this.grid, d = this.options, c = this.chart.renderer; if (!0 === (d.grid || {}).enabled) {
                        this.maxLabelDimensions = this.getMaxLabelDimensions(this.ticks, this.tickPositions); this.rightWall && this.rightWall.destroy(); if (this.grid && this.grid.isOuterAxis() && this.axisLine) {
                            var h = d.lineWidth; if (h) {
                                var f = this.getLinePath(h), k = f[0], v = f[1], r = ((this.tickSize("tick") || [1])[0] - 1) * (this.side === b.Side.top || this.side ===
                                    b.Side.left ? -1 : 1); "M" === k[0] && "L" === v[0] && (this.horiz ? (k[2] += r, v[2] += r) : (k[1] += r, v[1] += r)); this.grid.axisLineExtra ? this.grid.axisLineExtra.animate({ d: f }) : (this.grid.axisLineExtra = c.path(f).attr({ zIndex: 7 }).addClass("highcharts-axis-line").add(this.axisGroup), c.styledMode || this.grid.axisLineExtra.attr({ stroke: d.lineColor, "stroke-width": h })); this.axisLine[this.showAxis ? "show" : "hide"](!0)
                            }
                        } (a && a.columns || []).forEach(function (a) { a.render() })
                    }
                }; b.onAfterSetAxisTranslation = function () {
                    var a = this.tickPositions &&
                        this.tickPositions.info, b = this.options, d = b.grid || {}, c = this.userOptions.labels || {}; this.horiz && (!0 === d.enabled && this.series.forEach(function (a) { a.options.pointRange = 0 }), a && b.dateTimeLabelFormats && b.labels && !n(c.align) && (!1 === b.dateTimeLabelFormats[a.unitName].range || 1 < a.count) && (b.labels.align = "left", n(c.x) || (b.labels.x = 3)))
                }; b.onAfterSetOptions = function (a) {
                    var b = this.options; a = a.userOptions; var e = b && g.isObject(b.grid, !0) ? b.grid : {}; if (!0 === e.enabled) {
                        var d = y(!0, {
                            className: "highcharts-grid-axis " + (a.className ||
                                ""), dateTimeLabelFormats: { hour: { list: ["%H:%M", "%H"] }, day: { list: ["%A, %e. %B", "%a, %e. %b", "%E"] }, week: { list: ["Week %W", "W%W"] }, month: { list: ["%B", "%b", "%o"] } }, grid: { borderWidth: 1 }, labels: { padding: 2, style: { fontSize: "13px" } }, margin: 0, title: { text: null, reserveSpace: !1, rotation: 0 }, units: [["millisecond", [1, 10, 100]], ["second", [1, 10]], ["minute", [1, 5, 15]], ["hour", [1, 6]], ["day", [1]], ["week", [1]], ["month", [1]], ["year", null]]
                        }, a); "xAxis" === this.coll && (n(a.linkedTo) && !n(a.tickPixelInterval) && (d.tickPixelInterval =
                            350), n(a.tickPixelInterval) || !n(a.linkedTo) || n(a.tickPositioner) || n(a.tickInterval) || (d.tickPositioner = function (a, b) { var e = this.linkedParent && this.linkedParent.tickPositions && this.linkedParent.tickPositions.info; if (e) { var c, l = d.units; for (c = 0; c < l.length; c++)if (l[c][0] === e.unitName) { var h = c; break } if (l[h + 1]) { var f = l[h + 1][0]; var k = (l[h + 1][1] || [1])[0] } else "year" === e.unitName && (f = "year", k = 10 * e.count); e = z[f]; this.tickInterval = e * k; return this.getTimeTicks({ unitRange: e, count: k, unitName: f }, a, b, this.options.startOfWeek) } }));
                        y(!0, this.options, d); this.horiz && (b.minPadding = k(a.minPadding, 0), b.maxPadding = k(a.maxPadding, 0)); C(b.grid.borderWidth) && (b.tickWidth = b.lineWidth = e.borderWidth)
                    }
                }; b.onAfterSetOptions2 = function (a) { a = (a = a.userOptions) && a.grid || {}; var b = a.columns; a.enabled && b && y(!0, this.options, b[b.length - 1]) }; b.onAfterSetScale = function () { (this.grid.columns || []).forEach(function (a) { a.setScale() }) }; b.onAfterTickSize = function (b) {
                    var d = a.defaultLeftAxisOptions, e = this.horiz, c = this.maxLabelDimensions, h = this.options.grid; h =
                        void 0 === h ? {} : h; h.enabled && c && (d = 2 * Math.abs(d.labels.x), e = e ? h.cellHeight || d + c.height : d + c.width, A(b.tickSize) ? b.tickSize[0] = e : b.tickSize = [e, 0])
                }; b.onDestroy = function (a) { var b = this.grid; (b.columns || []).forEach(function (b) { b.destroy(a.keepEvents) }); b.columns = void 0 }; b.onInit = function (a) { a = a.userOptions || {}; var b = a.grid || {}; b.enabled && n(b.borderColor) && (a.tickColor = a.lineColor = b.borderColor); this.grid || (this.grid = new h(this)) }; b.onTrimTicks = function () {
                    var a = this.options, b = this.categories, d = this.tickPositions,
                    c = d[0], h = d[d.length - 1], f = this.linkedParent && this.linkedParent.min || this.min, k = this.linkedParent && this.linkedParent.max || this.max, r = this.tickInterval; !0 !== (a.grid || {}).enabled || b || !this.horiz && !this.isLinked || (c < f && c + r > f && !a.startOnTick && (d[0] = f), h > k && h - r < k && !a.endOnTick && (d[d.length - 1] = k))
                }; b.wrapUnsquish = function (a) { var b = this.options.grid; return !0 === (void 0 === b ? {} : b).enabled && this.categories ? this.tickInterval : a.apply(this, Array.prototype.slice.call(arguments, 1)) }; return b
            }(); (function (a) {
                a = a.Side ||
                (a.Side = {}); a[a.top = 0] = "top"; a[a.right = 1] = "right"; a[a.bottom = 2] = "bottom"; a[a.left = 3] = "left"
            })(p || (p = {})); p.compose(a); return p
        }); B(a, "modules/broken-axis.src.js", [a["parts/Axis.js"], a["parts/Globals.js"], a["parts/Utilities.js"], a["parts/Stacking.js"]], function (a, u, w, B) {
            var g = w.addEvent, q = w.find, t = w.fireEvent, n = w.isArray, m = w.isNumber, f = w.pick, A = u.Series, C = function () {
                function g(a) { this.hasBreaks = !1; this.axis = a } g.isInBreak = function (a, f) {
                    var c = a.repeat || Infinity, d = a.from, h = a.to - a.from; f = f >= d ? (f - d) % c :
                        c - (d - f) % c; return a.inclusive ? f <= h : f < h && 0 !== f
                }; g.lin2Val = function (a) { var f = this.brokenAxis; f = f && f.breakArray; if (!f) return a; var c; for (c = 0; c < f.length; c++) { var d = f[c]; if (d.from >= a) break; else d.to < a ? a += d.len : g.isInBreak(d, a) && (a += d.len) } return a }; g.val2Lin = function (a) { var f = this.brokenAxis; f = f && f.breakArray; if (!f) return a; var c = a, d; for (d = 0; d < f.length; d++) { var h = f[d]; if (h.to <= a) c -= h.len; else if (h.from >= a) break; else if (g.isInBreak(h, a)) { c -= a - h.from; break } } return c }; g.prototype.findBreakAt = function (a, f) {
                    return q(f,
                        function (c) { return c.from < a && a < c.to })
                }; g.prototype.isInAnyBreak = function (a, m) { var c = this.axis, d = c.options.breaks, h = d && d.length, p; if (h) { for (; h--;)if (g.isInBreak(d[h], a)) { var b = !0; p || (p = f(d[h].showPoints, !c.isXAxis)) } var e = b && m ? b && !p : b } return e }; g.prototype.setBreaks = function (k, m) {
                    var c = this, d = c.axis, h = n(k) && !!k.length; d.isDirty = c.hasBreaks !== h; c.hasBreaks = h; d.options.breaks = d.userOptions.breaks = k; d.forceRedraw = !0; d.series.forEach(function (a) { a.isDirty = !0 }); h || d.val2lin !== g.val2Lin || (delete d.val2lin,
                        delete d.lin2val); h && (d.userOptions.ordinal = !1, d.lin2val = g.lin2Val, d.val2lin = g.val2Lin, d.setExtremes = function (d, b, e, f, h) { if (c.hasBreaks) { for (var l, g = this.options.breaks; l = c.findBreakAt(d, g);)d = l.to; for (; l = c.findBreakAt(b, g);)b = l.from; b < d && (b = d) } a.prototype.setExtremes.call(this, d, b, e, f, h) }, d.setAxisTranslation = function (h) {
                            a.prototype.setAxisTranslation.call(this, h); c.unitLength = null; if (c.hasBreaks) {
                                h = d.options.breaks || []; var b = [], e = [], l = 0, k, p = d.userMin || d.min, m = d.userMax || d.max, n = f(d.pointRangePadding,
                                    0), v; h.forEach(function (a) { k = a.repeat || Infinity; g.isInBreak(a, p) && (p += a.to % k - p % k); g.isInBreak(a, m) && (m -= m % k - a.from % k) }); h.forEach(function (a) { x = a.from; for (k = a.repeat || Infinity; x - k > p;)x -= k; for (; x < p;)x += k; for (v = x; v < m; v += k)b.push({ value: v, move: "in" }), b.push({ value: v + (a.to - a.from), move: "out", size: a.breakSize }) }); b.sort(function (a, b) { return a.value === b.value ? ("in" === a.move ? 0 : 1) - ("in" === b.move ? 0 : 1) : a.value - b.value }); var r = 0; var x = p; b.forEach(function (a) {
                                        r += "in" === a.move ? 1 : -1; 1 === r && "in" === a.move && (x = a.value);
                                        0 === r && (e.push({ from: x, to: a.value, len: a.value - x - (a.size || 0) }), l += a.value - x - (a.size || 0))
                                    }); d.breakArray = c.breakArray = e; c.unitLength = m - p - l + n; t(d, "afterBreaks"); d.staticScale ? d.transA = d.staticScale : c.unitLength && (d.transA *= (m - d.min + n) / c.unitLength); n && (d.minPixelPadding = d.transA * d.minPointOffset); d.min = p; d.max = m
                            }
                        }); f(m, !0) && d.chart.redraw()
                }; return g
            }(); u = function () {
                function a() { } a.compose = function (a, n) {
                    a.keepProps.push("brokenAxis"); var c = A.prototype; c.drawBreaks = function (a, c) {
                        var d = this, b = d.points,
                        e, h, g, k; if (a && a.brokenAxis && a.brokenAxis.hasBreaks) { var n = a.brokenAxis; c.forEach(function (c) { e = n && n.breakArray || []; h = a.isXAxis ? a.min : f(d.options.threshold, a.min); b.forEach(function (b) { k = f(b["stack" + c.toUpperCase()], b[c]); e.forEach(function (d) { if (m(h) && m(k)) { g = !1; if (h < d.from && k > d.to || h > d.from && k < d.from) g = "pointBreak"; else if (h < d.from && k > d.from && k < d.to || h > d.from && k > d.to && k < d.from) g = "pointInBreak"; g && t(a, g, { point: b, brk: d }) } }) }) }) }
                    }; c.gappedPath = function () {
                        var a = this.currentDataGrouping, c = a && a.gapSize;
                        a = this.options.gapSize; var f = this.points.slice(), b = f.length - 1, e = this.yAxis, g; if (a && 0 < b) for ("value" !== this.options.gapUnit && (a *= this.basePointRange), c && c > a && c >= this.basePointRange && (a = c), g = void 0; b--;)g && !1 !== g.visible || (g = f[b + 1]), c = f[b], !1 !== g.visible && !1 !== c.visible && (g.x - c.x > a && (g = (c.x + g.x) / 2, f.splice(b + 1, 0, { isNull: !0, x: g }), e.stacking && this.options.stacking && (g = e.stacking.stacks[this.stackKey][g] = new B(e, e.options.stackLabels, !1, g, this.stack), g.total = 0)), g = c); return this.getGraphPath(f)
                    }; g(a, "init",
                        function () { this.brokenAxis || (this.brokenAxis = new C(this)) }); g(a, "afterInit", function () { "undefined" !== typeof this.brokenAxis && this.brokenAxis.setBreaks(this.options.breaks, !1) }); g(a, "afterSetTickPositions", function () { var a = this.brokenAxis; if (a && a.hasBreaks) { var c = this.tickPositions, f = this.tickPositions.info, b = [], e; for (e = 0; e < c.length; e++)a.isInAnyBreak(c[e]) || b.push(c[e]); this.tickPositions = b; this.tickPositions.info = f } }); g(a, "afterSetOptions", function () {
                            this.brokenAxis && this.brokenAxis.hasBreaks && (this.options.ordinal =
                                !1)
                        }); g(n, "afterGeneratePoints", function () { var a = this.options.connectNulls, c = this.points, f = this.xAxis, b = this.yAxis; if (this.isDirty) for (var e = c.length; e--;) { var g = c[e], k = !(null === g.y && !1 === a) && (f && f.brokenAxis && f.brokenAxis.isInAnyBreak(g.x, !0) || b && b.brokenAxis && b.brokenAxis.isInAnyBreak(g.y, !0)); g.visible = k ? !1 : !1 !== g.options.visible } }); g(n, "afterRender", function () { this.drawBreaks(this.xAxis, ["x"]); this.drawBreaks(this.yAxis, f(this.pointArrayMap, ["y"])) })
                }; return a
            }(); u.compose(a, A); return u
        }); B(a,
            "parts-gantt/TreeGridAxis.js", [a["parts/Axis.js"], a["parts/Tick.js"], a["parts-gantt/Tree.js"], a["parts-gantt/TreeGridTick.js"], a["mixins/tree-series.js"], a["parts/Utilities.js"]], function (a, u, w, B, g, q) {
                var t = q.addEvent, n = q.find, m = q.fireEvent, f = q.isNumber, A = q.isObject, C = q.isString, y = q.merge, k = q.pick, z = q.wrap, c; (function (a) {
                    function c(a, b) { var c = a.collapseStart || 0; a = a.collapseEnd || 0; a >= b && (c -= .5); return { from: c, to: a, showPoints: !1 } } function d(a, b, c) {
                        var d = [], e = [], f = {}, g = {}, h = -1, r = "boolean" === typeof b ?
                            b : !1; a = w.getTree(a, {
                                after: function (a) { a = g[a.pos]; var b = 0, c = 0; a.children.forEach(function (a) { c += (a.descendants || 0) + 1; b = Math.max((a.height || 0) + 1, b) }); a.descendants = c; a.height = b; a.collapsed && e.push(a) }, before: function (a) {
                                    var b = A(a.data, !0) ? a.data : {}, c = C(b.name) ? b.name : "", e = f[a.parent]; e = A(e, !0) ? g[e.pos] : null; var k = function (a) { return a.name === c }, l; r && A(e, !0) && (l = n(e.children, k)) ? (k = l.pos, l.nodes.push(a)) : k = h++; g[k] || (g[k] = l = { depth: e ? e.depth + 1 : 0, name: c, nodes: [a], children: [], pos: k }, -1 !== k && d.push(c), A(e,
                                        !0) && e.children.push(l)); C(a.id) && (f[a.id] = a); l && !0 === b.collapsed && (l.collapsed = !0); a.pos = k
                                }
                            }); g = function (a, b) { var c = function (a, d, e) { var f = d + (-1 === d ? 0 : b - 1), g = (f - d) / 2, h = d + g; a.nodes.forEach(function (a) { var b = a.data; A(b, !0) && (b.y = d + (b.seriesIndex || 0), delete b.seriesIndex); a.pos = h }); e[h] = a; a.pos = h; a.tickmarkOffset = g + .5; a.collapseStart = f + .5; a.children.forEach(function (a) { c(a, f + 1, e); f = (a.collapseEnd || 0) - .5 }); a.collapseEnd = f + .5; return e }; return c(a["-1"], -1, {}) }(g, c); return {
                                categories: d, mapOfIdToNode: f,
                                mapOfPosToGridNode: g, collapsedNodes: e, tree: a
                            }
                    } function b(a) {
                        a.target.axes.filter(function (a) { return "treegrid" === a.options.type }).forEach(function (b) {
                            var c = b.options || {}, e = c.labels, f = c.uniqueNames, h = 0; if (!b.treeGrid.mapOfPosToGridNode || b.series.some(function (a) { return !a.hasRendered || a.isDirtyData || a.isDirty })) c = b.series.reduce(function (a, b) { b.visible && ((b.options.data || []).forEach(function (b) { A(b, !0) && (b.seriesIndex = h, a.push(b)) }), !0 === f && h++); return a }, []), c = d(c, f || !1, !0 === f ? h : 1), b.categories = c.categories,
                                b.treeGrid.mapOfPosToGridNode = c.mapOfPosToGridNode, b.hasNames = !0, b.treeGrid.tree = c.tree, b.series.forEach(function (a) { var b = (a.options.data || []).map(function (a) { return A(a, !0) ? y(a) : a }); a.visible && a.setData(b, !1) }), b.treeGrid.mapOptionsToLevel = g.getLevelOptions({ defaults: e, from: 1, levels: e && e.levels, to: b.treeGrid.tree && b.treeGrid.tree.height }), "beforeRender" === a.type && (b.treeGrid.collapsedNodes = c.collapsedNodes)
                        })
                    } function e(a, b) {
                        var c = this.treeGrid.mapOptionsToLevel || {}, d = this.ticks, e = d[b], f; if ("treegrid" ===
                            this.options.type && this.treeGrid.mapOfPosToGridNode) { var g = this.treeGrid.mapOfPosToGridNode[b]; (c = c[g.depth]) && (f = { labels: c }); e ? (e.parameters.category = g.name, e.options = f, e.addLabel()) : d[b] = new u(this, b, void 0, void 0, { category: g.name, tickmarkOffset: g.tickmarkOffset, options: f }) } else a.apply(this, Array.prototype.slice.call(arguments, 1))
                    } function l(a) {
                        var b = this.options; b = (b = b && b.labels) && f(b.indentation) ? b.indentation : 0; var c = a.apply(this, Array.prototype.slice.call(arguments, 1)); if ("treegrid" === this.options.type &&
                            this.treeGrid.mapOfPosToGridNode) { var d = this.treeGrid.mapOfPosToGridNode[-1].height || 0; c.width += b * (d - 1) } return c
                    } function q(a, c, e) {
                        var f = this, g = "treegrid" === e.type; f.treeGrid || (f.treeGrid = new H(f)); g && (t(c, "beforeRender", b), t(c, "beforeRedraw", b), t(c, "addSeries", function (a) { a.options.data && (a = d(a.options.data, e.uniqueNames || !1, 1), f.treeGrid.collapsedNodes = (f.treeGrid.collapsedNodes || []).concat(a.collapsedNodes)) }), t(f, "foundExtremes", function () {
                            f.treeGrid.collapsedNodes && f.treeGrid.collapsedNodes.forEach(function (a) {
                                var b =
                                    f.treeGrid.collapse(a); f.brokenAxis && (f.brokenAxis.setBreaks(b, !1), f.treeGrid.collapsedNodes && (f.treeGrid.collapsedNodes = f.treeGrid.collapsedNodes.filter(function (b) { return a.collapseStart !== b.collapseStart || a.collapseEnd !== b.collapseEnd })))
                            })
                        }), t(f, "afterBreaks", function () { var a; "yAxis" === f.coll && !f.staticScale && (null === (a = f.chart.options.chart) || void 0 === a ? 0 : a.height) && (f.isDirty = !0) }), e = y({
                            grid: { enabled: !0 }, labels: {
                                align: "left", levels: [{ level: void 0 }, { level: 1, style: { fontWeight: "bold" } }], symbol: {
                                    type: "triangle",
                                    x: -5, y: -5, height: 10, width: 10, padding: 5
                                }
                            }, uniqueNames: !1
                        }, e, { reversed: !0, grid: { columns: void 0 } })); a.apply(f, [c, e]); g && (f.hasNames = !0, f.options.showLastLabel = !0)
                    } function E(a) {
                        var b = this.options; "treegrid" === b.type ? (this.min = k(this.userMin, b.min, this.dataMin), this.max = k(this.userMax, b.max, this.dataMax), m(this, "foundExtremes"), this.setAxisTranslation(!0), this.tickmarkOffset = .5, this.tickInterval = 1, this.tickPositions = this.treeGrid.mapOfPosToGridNode ? this.treeGrid.getTickPositions() : []) : a.apply(this, Array.prototype.slice.call(arguments,
                            1))
                    } var G = !1; a.compose = function (a) { G || (z(a.prototype, "generateTick", e), z(a.prototype, "getMaxLabelDimensions", l), z(a.prototype, "init", q), z(a.prototype, "setTickInterval", E), B.compose(u), G = !0) }; var H = function () {
                        function a(a) { this.axis = a } a.prototype.collapse = function (a) { var b = this.axis, d = b.options.breaks || []; a = c(a, b.max); d.push(a); return d }; a.prototype.expand = function (a) {
                            var b = this.axis, d = b.options.breaks || [], e = c(a, b.max); return d.reduce(function (a, b) { b.to === e.to && b.from === e.from || a.push(b); return a },
                                [])
                        }; a.prototype.getTickPositions = function () { var a = this.axis; return Object.keys(a.treeGrid.mapOfPosToGridNode || {}).reduce(function (b, c) { c = +c; !(a.min <= c && a.max >= c) || a.brokenAxis && a.brokenAxis.isInAnyBreak(c) || b.push(c); return b }, []) }; a.prototype.isCollapsed = function (a) { var b = this.axis, d = b.options.breaks || [], e = c(a, b.max); return d.some(function (a) { return a.from === e.from && a.to === e.to }) }; a.prototype.toggleCollapse = function (a) { return this.isCollapsed(a) ? this.expand(a) : this.collapse(a) }; return a
                    }(); a.Additions =
                        H
                })(c || (c = {})); a.prototype.utils = { getNode: w.getNode }; c.compose(a); return c
            }); B(a, "masters/modules/treegrid.src.js", [], function () { })
});
//# sourceMappingURL=treegrid.js.map