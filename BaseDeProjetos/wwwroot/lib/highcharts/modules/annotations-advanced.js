/*
 Highcharts JS v8.1.2 (2020-06-16)

 Annotations module

 (c) 2009-2019 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (d) { "object" === typeof module && module.exports ? (d["default"] = d, module.exports = d) : "function" === typeof define && define.amd ? define("highcharts/modules/annotations-advanced", ["highcharts"], function (t) { d(t); d.Highcharts = t; return d }) : d("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (d) {
    function t(d, f, l, g) { d.hasOwnProperty(f) || (d[f] = g.apply(null, l)) } d = d ? d._modules : {}; t(d, "annotations/eventEmitterMixin.js", [d["parts/Globals.js"], d["parts/Utilities.js"]], function (d, f) {
        var l = f.addEvent,
        g = f.fireEvent, r = f.inArray, e = f.objectEach, k = f.pick, b = f.removeEvent; return {
            addEvents: function () {
                var a = this, c = function (c) { l(c, d.isTouchDevice ? "touchstart" : "mousedown", function (c) { a.onMouseDown(c) }) }; c(this.graphic.element); (a.labels || []).forEach(function (a) { a.options.useHTML && a.graphic.text && c(a.graphic.text.element) }); e(a.options.events, function (c, b) {
                    var h = function (h) { "click" === b && a.cancelClick || c.call(a, a.chart.pointer.normalize(h), a.target) }; if (-1 === r(b, a.nonDOMEvents || [])) a.graphic.on(b, h); else l(a,
                        b, h)
                }); if (a.options.draggable && (l(a, d.isTouchDevice ? "touchmove" : "drag", a.onDrag), !a.graphic.renderer.styledMode)) { var b = { cursor: { x: "ew-resize", y: "ns-resize", xy: "move" }[a.options.draggable] }; a.graphic.css(b); (a.labels || []).forEach(function (a) { a.options.useHTML && a.graphic.text && a.graphic.text.css(b) }) } a.isUpdating || g(a, "add")
            }, removeDocEvents: function () { this.removeDrag && (this.removeDrag = this.removeDrag()); this.removeMouseUp && (this.removeMouseUp = this.removeMouseUp()) }, onMouseDown: function (a) {
                var c = this,
                b = c.chart.pointer; a.preventDefault && a.preventDefault(); if (2 !== a.button) {
                    a = b.normalize(a); var p = a.chartX; var m = a.chartY; c.cancelClick = !1; c.chart.hasDraggedAnnotation = !0; c.removeDrag = l(d.doc, d.isTouchDevice ? "touchmove" : "mousemove", function (a) { c.hasDragged = !0; a = b.normalize(a); a.prevChartX = p; a.prevChartY = m; g(c, "drag", a); p = a.chartX; m = a.chartY }); c.removeMouseUp = l(d.doc, d.isTouchDevice ? "touchend" : "mouseup", function (a) {
                        c.cancelClick = c.hasDragged; c.hasDragged = !1; c.chart.hasDraggedAnnotation = !1; g(k(c.target,
                            c), "afterUpdate"); c.onMouseUp(a)
                    })
                }
            }, onMouseUp: function (a) { var c = this.chart; a = this.target || this; var b = c.options.annotations; c = c.annotations.indexOf(a); this.removeDocEvents(); b[c] = a.options }, onDrag: function (a) {
                if (this.chart.isInsidePlot(a.chartX - this.chart.plotLeft, a.chartY - this.chart.plotTop)) {
                    var c = this.mouseMoveToTranslation(a); "x" === this.options.draggable && (c.y = 0); "y" === this.options.draggable && (c.x = 0); this.points.length ? this.translate(c.x, c.y) : (this.shapes.forEach(function (a) { a.translate(c.x, c.y) }),
                        this.labels.forEach(function (a) { a.translate(c.x, c.y) })); this.redraw(!1)
                }
            }, mouseMoveToRadians: function (a, c, b) { var h = a.prevChartY - b, m = a.prevChartX - c; b = a.chartY - b; a = a.chartX - c; this.chart.inverted && (c = m, m = h, h = c, c = a, a = b, b = c); return Math.atan2(b, a) - Math.atan2(h, m) }, mouseMoveToTranslation: function (a) { var c = a.chartX - a.prevChartX; a = a.chartY - a.prevChartY; if (this.chart.inverted) { var b = a; a = c; c = b } return { x: c, y: a } }, mouseMoveToScale: function (a, c, b) {
                c = (a.chartX - c || 1) / (a.prevChartX - c || 1); a = (a.chartY - b || 1) / (a.prevChartY -
                    b || 1); this.chart.inverted && (b = a, a = c, c = b); return { x: c, y: a }
            }, destroy: function () { this.removeDocEvents(); b(this); this.hcEvents = null }
        }
    }); t(d, "annotations/ControlPoint.js", [d["parts/Utilities.js"], d["annotations/eventEmitterMixin.js"]], function (d, f) {
        var l = d.merge, g = d.pick; return function () {
            function d(e, k, b, a) {
                this.addEvents = f.addEvents; this.graphic = void 0; this.mouseMoveToRadians = f.mouseMoveToRadians; this.mouseMoveToScale = f.mouseMoveToScale; this.mouseMoveToTranslation = f.mouseMoveToTranslation; this.onDrag =
                    f.onDrag; this.onMouseDown = f.onMouseDown; this.onMouseUp = f.onMouseUp; this.removeDocEvents = f.removeDocEvents; this.nonDOMEvents = ["drag"]; this.chart = e; this.target = k; this.options = b; this.index = g(b.index, a)
            } d.prototype.setVisibility = function (e) { this.graphic.attr("visibility", e ? "visible" : "hidden"); this.options.visible = e }; d.prototype.render = function () {
                var e = this.chart, k = this.options; this.graphic = e.renderer.symbol(k.symbol, 0, 0, k.width, k.height).add(e.controlPointsGroup).css(k.style); this.setVisibility(k.visible);
                this.addEvents()
            }; d.prototype.redraw = function (e) { this.graphic[e ? "animate" : "attr"](this.options.positioner.call(this, this.target)) }; d.prototype.destroy = function () { f.destroy.call(this); this.graphic && (this.graphic = this.graphic.destroy()); this.options = this.target = this.chart = null }; d.prototype.update = function (e) { var k = this.chart, b = this.target, a = this.index; e = l(!0, this.options, e); this.destroy(); this.constructor(k, b, e, a); this.render(k.controlPointsGroup); this.redraw() }; return d
        }()
    }); t(d, "annotations/MockPoint.js",
        [d["parts/Globals.js"], d["parts/Utilities.js"]], function (d, f) {
            var l = f.defined, g = f.fireEvent; return function () {
                function f(e, k, b) { this.y = this.x = this.plotY = this.plotX = this.isInside = void 0; this.mock = !0; this.series = { visible: !0, chart: e, getPlotBox: d.Series.prototype.getPlotBox }; this.target = k || null; this.options = b; this.applyOptions(this.getOptions()) } f.fromPoint = function (e) { return new f(e.series.chart, null, { x: e.x, y: e.y, xAxis: e.series.xAxis, yAxis: e.series.yAxis }) }; f.pointToPixels = function (e, k) {
                    var b = e.series,
                    a = b.chart, c = e.plotX, h = e.plotY; a.inverted && (e.mock ? (c = e.plotY, h = e.plotX) : (c = a.plotWidth - e.plotY, h = a.plotHeight - e.plotX)); b && !k && (e = b.getPlotBox(), c += e.translateX, h += e.translateY); return { x: c, y: h }
                }; f.pointToOptions = function (e) { return { x: e.x, y: e.y, xAxis: e.series.xAxis, yAxis: e.series.yAxis } }; f.prototype.hasDynamicOptions = function () { return "function" === typeof this.options }; f.prototype.getOptions = function () { return this.hasDynamicOptions() ? this.options(this.target) : this.options }; f.prototype.applyOptions = function (e) {
                    this.command =
                    e.command; this.setAxis(e, "x"); this.setAxis(e, "y"); this.refresh()
                }; f.prototype.setAxis = function (e, k) { k += "Axis"; e = e[k]; var b = this.series.chart; this.series[k] = e instanceof d.Axis ? e : l(e) ? b[k][e] || b.get(e) : null }; f.prototype.toAnchor = function () { var e = [this.plotX, this.plotY, 0, 0]; this.series.chart.inverted && (e[0] = this.plotY, e[1] = this.plotX); return e }; f.prototype.getLabelConfig = function () { return { x: this.x, y: this.y, point: this } }; f.prototype.isInsidePlot = function () {
                    var e = this.plotX, k = this.plotY, b = this.series.xAxis,
                    a = this.series.yAxis, c = { x: e, y: k, isInsidePlot: !0 }; b && (c.isInsidePlot = l(e) && 0 <= e && e <= b.len); a && (c.isInsidePlot = c.isInsidePlot && l(k) && 0 <= k && k <= a.len); g(this.series.chart, "afterIsInsidePlot", c); return c.isInsidePlot
                }; f.prototype.refresh = function () { var e = this.series, k = e.xAxis; e = e.yAxis; var b = this.getOptions(); k ? (this.x = b.x, this.plotX = k.toPixels(b.x, !0)) : (this.x = null, this.plotX = b.x); e ? (this.y = b.y, this.plotY = e.toPixels(b.y, !0)) : (this.y = null, this.plotY = b.y); this.isInside = this.isInsidePlot() }; f.prototype.translate =
                    function (e, k, b, a) { this.hasDynamicOptions() || (this.plotX += b, this.plotY += a, this.refreshOptions()) }; f.prototype.scale = function (e, k, b, a) { if (!this.hasDynamicOptions()) { var c = this.plotY * a; this.plotX = (1 - b) * e + this.plotX * b; this.plotY = (1 - a) * k + c; this.refreshOptions() } }; f.prototype.rotate = function (e, k, b) { if (!this.hasDynamicOptions()) { var a = Math.cos(b); b = Math.sin(b); var c = this.plotX, h = this.plotY; c -= e; h -= k; this.plotX = c * a - h * b + e; this.plotY = c * b + h * a + k; this.refreshOptions() } }; f.prototype.refreshOptions = function () {
                        var e =
                            this.series, k = e.xAxis; e = e.yAxis; this.x = this.options.x = k ? this.options.x = k.toValue(this.plotX, !0) : this.plotX; this.y = this.options.y = e ? e.toValue(this.plotY, !0) : this.plotY
                    }; return f
            }()
        }); t(d, "annotations/controllable/controllableMixin.js", [d["annotations/ControlPoint.js"], d["annotations/MockPoint.js"], d["parts/Tooltip.js"], d["parts/Utilities.js"]], function (d, f, l, g) {
            var r = g.isObject, e = g.isString, k = g.merge, b = g.splat; return {
                init: function (a, c, b) {
                    this.annotation = a; this.chart = a.chart; this.options = c; this.points =
                        []; this.controlPoints = []; this.index = b; this.linkPoints(); this.addControlPoints()
                }, attr: function () { this.graphic.attr.apply(this.graphic, arguments) }, getPointsOptions: function () { var a = this.options; return a.points || a.point && b(a.point) }, attrsFromOptions: function (a) { var c = this.constructor.attrsMap, b = {}, p, m = this.chart.styledMode; for (p in a) { var e = c[p]; !e || m && -1 !== ["fill", "stroke", "stroke-width"].indexOf(e) || (b[e] = a[p]) } return b }, anchor: function (a) {
                    var c = a.series.getPlotBox(); a = a.mock ? a.toAnchor() : l.prototype.getAnchor.call({ chart: a.series.chart },
                        a); a = { x: a[0] + (this.options.x || 0), y: a[1] + (this.options.y || 0), height: a[2] || 0, width: a[3] || 0 }; return { relativePosition: a, absolutePosition: k(a, { x: a.x + c.translateX, y: a.y + c.translateY }) }
                }, point: function (a, c) { if (a && a.series) return a; c && null !== c.series || (r(a) ? c = new f(this.chart, this, a) : e(a) ? c = this.chart.get(a) || null : "function" === typeof a && (c = a.call(c, this), c = c.series ? c : new f(this.chart, this, a))); return c }, linkPoints: function () {
                    var a = this.getPointsOptions(), c = this.points, b = a && a.length || 0, p; for (p = 0; p < b; p++) {
                        var m =
                            this.point(a[p], c[p]); if (!m) { c.length = 0; return } m.mock && m.refresh(); c[p] = m
                    } return c
                }, addControlPoints: function () { var a = this.options.controlPoints; (a || []).forEach(function (c, b) { c = k(this.options.controlPointOptions, c); c.index || (c.index = b); a[b] = c; this.controlPoints.push(new d(this.chart, this, c)) }, this) }, shouldBeDrawn: function () { return !!this.points.length }, render: function (a) { this.controlPoints.forEach(function (a) { a.render() }) }, redraw: function (a) { this.controlPoints.forEach(function (c) { c.redraw(a) }) }, transform: function (a,
                    c, b, p, m) { if (this.chart.inverted) { var h = c; c = b; b = h } this.points.forEach(function (h, e) { this.transformPoint(a, c, b, p, m, e) }, this) }, transformPoint: function (a, c, b, p, m, e) { var h = this.points[e]; h.mock || (h = this.points[e] = f.fromPoint(h)); h[a](c, b, p, m) }, translate: function (a, c) { this.transform("translate", null, null, a, c) }, translatePoint: function (a, c, b) { this.transformPoint("translate", null, null, a, c, b) }, translateShape: function (a, b) {
                        var c = this.annotation.chart, p = this.annotation.userOptions, m = c.annotations.indexOf(this.annotation);
                        c = c.options.annotations[m]; this.translatePoint(a, b, 0); c[this.collection][this.index].point = this.options.point; p[this.collection][this.index].point = this.options.point
                    }, rotate: function (a, c, b) { this.transform("rotate", a, c, b) }, scale: function (a, b, h, p) { this.transform("scale", a, b, h, p) }, setControlPointsVisibility: function (a) { this.controlPoints.forEach(function (b) { b.setVisibility(a) }) }, destroy: function () {
                        this.graphic && (this.graphic = this.graphic.destroy()); this.tracker && (this.tracker = this.tracker.destroy());
                        this.controlPoints.forEach(function (a) { a.destroy() }); this.options = this.controlPoints = this.points = this.chart = null; this.annotation && (this.annotation = null)
                    }, update: function (a) { var b = this.annotation; a = k(!0, this.options, a); var h = this.graphic.parentGroup; this.destroy(); this.constructor(b, a); this.render(h); this.redraw() }
            }
        }); t(d, "annotations/controllable/markerMixin.js", [d["parts/Globals.js"], d["parts/Utilities.js"]], function (d, f) {
            var l = f.addEvent, g = f.defined, r = f.merge, e = f.objectEach, k = f.uniqueKey, b = {
                arrow: {
                    tagName: "marker",
                    render: !1, id: "arrow", refY: 5, refX: 9, markerWidth: 10, markerHeight: 10, children: [{ tagName: "path", d: "M 0 0 L 10 5 L 0 10 Z", strokeWidth: 0 }]
                }, "reverse-arrow": { tagName: "marker", render: !1, id: "reverse-arrow", refY: 5, refX: 1, markerWidth: 10, markerHeight: 10, children: [{ tagName: "path", d: "M 0 5 L 10 0 L 10 10 Z", strokeWidth: 0 }] }
            }; d.SVGRenderer.prototype.addMarker = function (a, b) {
                var c = { id: a }, p = { stroke: b.color || "none", fill: b.color || "rgba(0, 0, 0, 0.75)" }; c.children = b.children.map(function (a) { return r(p, a) }); b = this.definition(r(!0,
                    { markerWidth: 20, markerHeight: 20, refX: 0, refY: 0, orient: "auto" }, b, c)); b.id = a; return b
            }; f = function (a) { return function (b) { this.attr(a, "url(#" + b + ")") } }; f = {
                markerEndSetter: f("marker-end"), markerStartSetter: f("marker-start"), setItemMarkers: function (a) {
                    var b = a.options, h = a.chart, p = h.options.defs, m = b.fill, e = g(m) && "none" !== m ? m : b.stroke;["markerStart", "markerEnd"].forEach(function (c) {
                        var m = b[c], n; if (m) {
                            for (n in p) { var d = p[n]; if (m === d.id && "marker" === d.tagName) { var f = d; break } } f && (m = a[c] = h.renderer.addMarker((b.id ||
                                k()) + "-" + f.id, r(f, { color: e })), a.attr(c, m.attr("id")))
                        }
                    })
                }
            }; l(d.Chart, "afterGetContainer", function () { this.options.defs = r(b, this.options.defs || {}); e(this.options.defs, function (a) { "marker" === a.tagName && !1 !== a.render && this.renderer.addMarker(a.id, a) }, this) }); return f
        }); t(d, "annotations/controllable/ControllablePath.js", [d["annotations/controllable/controllableMixin.js"], d["parts/Globals.js"], d["annotations/controllable/markerMixin.js"], d["parts/Utilities.js"]], function (d, f, l, g) {
            var r = g.extend; g = g.merge;
            var e = "rgba(192,192,192," + (f.svg ? .0001 : .002) + ")"; f = function (e, b, a) { this.init(e, b, a); this.collection = "shapes" }; f.attrsMap = { dashStyle: "dashstyle", strokeWidth: "stroke-width", stroke: "stroke", fill: "fill", zIndex: "zIndex" }; g(!0, f.prototype, d, {
                type: "path", setMarkers: l.setItemMarkers, toD: function () {
                    var e = this.options.d; if (e) return "function" === typeof e ? e.call(this) : e; e = this.points; var b = e.length, a = b, c = e[0], h = a && this.anchor(c).absolutePosition, p = 0, m = []; if (h) for (m.push(["M", h.x, h.y]); ++p < b && a;)c = e[p], a = c.command ||
                        "L", h = this.anchor(c).absolutePosition, "M" === a ? m.push([a, h.x, h.y]) : "L" === a ? m.push([a, h.x, h.y]) : "Z" === a && m.push([a]), a = c.series.visible; return a ? this.chart.renderer.crispLine(m, this.graphic.strokeWidth()) : null
                }, shouldBeDrawn: function () { return d.shouldBeDrawn.call(this) || !!this.options.d }, render: function (k) {
                    var b = this.options, a = this.attrsFromOptions(b); this.graphic = this.annotation.chart.renderer.path([["M", 0, 0]]).attr(a).add(k); b.className && this.graphic.addClass(b.className); this.tracker = this.annotation.chart.renderer.path([["M",
                        0, 0]]).addClass("highcharts-tracker-line").attr({ zIndex: 2 }).add(k); this.annotation.chart.styledMode || this.tracker.attr({ "stroke-linejoin": "round", stroke: e, fill: e, "stroke-width": this.graphic.strokeWidth() + 2 * b.snap }); d.render.call(this); r(this.graphic, { markerStartSetter: l.markerStartSetter, markerEndSetter: l.markerEndSetter }); this.setMarkers(this)
                }, redraw: function (e) {
                    var b = this.toD(), a = e ? "animate" : "attr"; b ? (this.graphic[a]({ d: b }), this.tracker[a]({ d: b })) : (this.graphic.attr({ d: "M 0 -9000000000" }), this.tracker.attr({ d: "M 0 -9000000000" }));
                    this.graphic.placed = this.tracker.placed = !!b; d.redraw.call(this, e)
                }
            }); return f
        }); t(d, "annotations/controllable/ControllableRect.js", [d["annotations/controllable/controllableMixin.js"], d["annotations/controllable/ControllablePath.js"], d["parts/Utilities.js"]], function (d, f, l) {
            l = l.merge; var g = function (d, e, k) { this.init(d, e, k); this.collection = "shapes" }; g.attrsMap = l(f.attrsMap, { width: "width", height: "height" }); l(!0, g.prototype, d, {
                type: "rect", translate: d.translateShape, render: function (f) {
                    var e = this.attrsFromOptions(this.options);
                    this.graphic = this.annotation.chart.renderer.rect(0, -9E9, 0, 0).attr(e).add(f); d.render.call(this)
                }, redraw: function (f) { var e = this.anchor(this.points[0]).absolutePosition; if (e) this.graphic[f ? "animate" : "attr"]({ x: e.x, y: e.y, width: this.options.width, height: this.options.height }); else this.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!e; d.redraw.call(this, f) }
            }); return g
        }); t(d, "annotations/controllable/ControllableCircle.js", [d["annotations/controllable/controllableMixin.js"], d["annotations/controllable/ControllablePath.js"],
        d["parts/Utilities.js"]], function (d, f, l) {
            l = l.merge; var g = function (d, e, k) { this.init(d, e, k); this.collection = "shapes" }; g.attrsMap = l(f.attrsMap, { r: "r" }); l(!0, g.prototype, d, {
                type: "circle", translate: d.translateShape, render: function (f) { var e = this.attrsFromOptions(this.options); this.graphic = this.annotation.chart.renderer.circle(0, -9E9, 0).attr(e).add(f); d.render.call(this) }, redraw: function (f) {
                    var e = this.anchor(this.points[0]).absolutePosition; if (e) this.graphic[f ? "animate" : "attr"]({ x: e.x, y: e.y, r: this.options.r });
                    else this.graphic.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!e; d.redraw.call(this, f)
                }, setRadius: function (d) { this.options.r = d }
            }); return g
        }); t(d, "annotations/controllable/ControllableLabel.js", [d["annotations/controllable/controllableMixin.js"], d["parts/Globals.js"], d["annotations/MockPoint.js"], d["parts/Tooltip.js"], d["parts/Utilities.js"]], function (d, f, l, g, r) {
            var e = r.extend, k = r.format, b = r.isNumber, a = r.merge, c = r.pick, h = function (a, b, c) { this.init(a, b, c); this.collection = "labels" }; h.shapesWithoutBackground =
                ["connector"]; h.alignedPosition = function (a, b) { var c = a.align, h = a.verticalAlign, p = (b.x || 0) + (a.x || 0), m = (b.y || 0) + (a.y || 0), e, d; "right" === c ? e = 1 : "center" === c && (e = 2); e && (p += (b.width - (a.width || 0)) / e); "bottom" === h ? d = 1 : "middle" === h && (d = 2); d && (m += (b.height - (a.height || 0)) / d); return { x: Math.round(p), y: Math.round(m) } }; h.justifiedOptions = function (a, b, c, h) {
                    var p = c.align, e = c.verticalAlign, m = b.box ? 0 : b.padding || 0, d = b.getBBox(); b = { align: p, verticalAlign: e, x: c.x, y: c.y, width: b.width, height: b.height }; c = h.x - a.plotLeft; var n =
                        h.y - a.plotTop; h = c + m; 0 > h && ("right" === p ? b.align = "left" : b.x = -h); h = c + d.width - m; h > a.plotWidth && ("left" === p ? b.align = "right" : b.x = a.plotWidth - h); h = n + m; 0 > h && ("bottom" === e ? b.verticalAlign = "top" : b.y = -h); h = n + d.height - m; h > a.plotHeight && ("top" === e ? b.verticalAlign = "bottom" : b.y = a.plotHeight - h); return b
                }; h.attrsMap = { backgroundColor: "fill", borderColor: "stroke", borderWidth: "stroke-width", zIndex: "zIndex", borderRadius: "r", padding: "padding" }; a(!0, h.prototype, d, {
                    translatePoint: function (a, b) {
                        d.translatePoint.call(this, a,
                            b, 0)
                    }, translate: function (a, b) { var c = this.annotation.chart, h = this.annotation.userOptions, e = c.annotations.indexOf(this.annotation); e = c.options.annotations[e]; c.inverted && (c = a, a = b, b = c); this.options.x += a; this.options.y += b; e[this.collection][this.index].x = this.options.x; e[this.collection][this.index].y = this.options.y; h[this.collection][this.index].x = this.options.x; h[this.collection][this.index].y = this.options.y }, render: function (a) {
                        var b = this.options, c = this.attrsFromOptions(b), e = b.style; this.graphic = this.annotation.chart.renderer.label("",
                            0, -9999, b.shape, null, null, b.useHTML, null, "annotation-label").attr(c).add(a); this.annotation.chart.styledMode || ("contrast" === e.color && (e.color = this.annotation.chart.renderer.getContrast(-1 < h.shapesWithoutBackground.indexOf(b.shape) ? "#FFFFFF" : b.backgroundColor)), this.graphic.css(b.style).shadow(b.shadow)); b.className && this.graphic.addClass(b.className); this.graphic.labelrank = b.labelrank; d.render.call(this)
                    }, redraw: function (a) {
                        var b = this.options, c = this.text || b.format || b.text, h = this.graphic, e = this.points[0];
                        h.attr({ text: c ? k(c, e.getLabelConfig(), this.annotation.chart) : b.formatter.call(e, this) }); b = this.anchor(e); (c = this.position(b)) ? (h.alignAttr = c, c.anchorX = b.absolutePosition.x, c.anchorY = b.absolutePosition.y, h[a ? "animate" : "attr"](c)) : h.attr({ x: 0, y: -9999 }); h.placed = !!c; d.redraw.call(this, a)
                    }, anchor: function () { var a = d.anchor.apply(this, arguments), b = this.options.x || 0, c = this.options.y || 0; a.absolutePosition.x -= b; a.absolutePosition.y -= c; a.relativePosition.x -= b; a.relativePosition.y -= c; return a }, position: function (a) {
                        var b =
                            this.graphic, p = this.annotation.chart, d = this.points[0], q = this.options, k = a.absolutePosition, f = a.relativePosition; if (a = d.series.visible && l.prototype.isInsidePlot.call(d)) {
                                if (q.distance) var v = g.prototype.getPosition.call({ chart: p, distance: c(q.distance, 16) }, b.width, b.height, { plotX: f.x, plotY: f.y, negative: d.negative, ttBelow: d.ttBelow, h: f.height || f.width }); else q.positioner ? v = q.positioner.call(this) : (d = { x: k.x, y: k.y, width: 0, height: 0 }, v = h.alignedPosition(e(q, { width: b.width, height: b.height }), d), "justify" ===
                                    this.options.overflow && (v = h.alignedPosition(h.justifiedOptions(p, b, q, v), d))); q.crop && (q = v.x - p.plotLeft, d = v.y - p.plotTop, a = p.isInsidePlot(q, d) && p.isInsidePlot(q + b.width, d + b.height))
                            } return a ? v : null
                    }
                }); f.SVGRenderer.prototype.symbols.connector = function (a, c, h, e, d) {
                    var p = d && d.anchorX; d = d && d.anchorY; var m = h / 2; if (b(p) && b(d)) { var k = [["M", p, d]]; var n = c - d; 0 > n && (n = -e - n); n < h && (m = p < a + h / 2 ? n : h - n); d > c + e ? k.push(["L", a + m, c + e]) : d < c ? k.push(["L", a + m, c]) : p < a ? k.push(["L", a, c + e / 2]) : p > a + h && k.push(["L", a + h, c + e / 2]) } return k ||
                        []
                }; return h
        }); t(d, "annotations/controllable/ControllableImage.js", [d["annotations/controllable/ControllableLabel.js"], d["annotations/controllable/controllableMixin.js"], d["parts/Utilities.js"]], function (d, f, l) {
            l = l.merge; var g = function (d, e, k) { this.init(d, e, k); this.collection = "shapes" }; g.attrsMap = { width: "width", height: "height", zIndex: "zIndex" }; l(!0, g.prototype, f, {
                type: "image", translate: f.translateShape, render: function (d) {
                    var e = this.attrsFromOptions(this.options), k = this.options; this.graphic = this.annotation.chart.renderer.image(k.src,
                        0, -9E9, k.width, k.height).attr(e).add(d); this.graphic.width = k.width; this.graphic.height = k.height; f.render.call(this)
                }, redraw: function (g) { var e = this.anchor(this.points[0]); if (e = d.prototype.position.call(this, e)) this.graphic[g ? "animate" : "attr"]({ x: e.x, y: e.y }); else this.graphic.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!e; f.redraw.call(this, g) }
            }); return g
        }); t(d, "annotations/annotations.src.js", [d["parts/Chart.js"], d["annotations/controllable/controllableMixin.js"], d["annotations/controllable/ControllableRect.js"],
        d["annotations/controllable/ControllableCircle.js"], d["annotations/controllable/ControllablePath.js"], d["annotations/controllable/ControllableImage.js"], d["annotations/controllable/ControllableLabel.js"], d["annotations/ControlPoint.js"], d["annotations/eventEmitterMixin.js"], d["parts/Globals.js"], d["annotations/MockPoint.js"], d["parts/Pointer.js"], d["parts/Utilities.js"]], function (d, f, l, g, r, e, k, b, a, c, h, p, m) {
            d = d.prototype; var B = m.addEvent, n = m.defined, q = m.destroyObjectProperties, x = m.erase, A = m.extend,
                v = m.find, w = m.fireEvent, u = m.merge, y = m.pick, C = m.splat; m = m.wrap; var z = function () {
                    function c(a, b) {
                        this.annotation = void 0; this.coll = "annotations"; this.shapesGroup = this.labelsGroup = this.labelCollector = this.group = this.graphic = this.collection = void 0; this.chart = a; this.points = []; this.controlPoints = []; this.coll = "annotations"; this.labels = []; this.shapes = []; this.options = u(this.defaultOptions, b); this.userOptions = b; b = this.getLabelsAndShapesOptions(this.options, b); this.options.labels = b.labels; this.options.shapes =
                            b.shapes; this.init(a, this.options)
                    } c.prototype.init = function () { this.linkPoints(); this.addControlPoints(); this.addShapes(); this.addLabels(); this.setLabelCollector() }; c.prototype.getLabelsAndShapesOptions = function (a, b) { var c = {};["labels", "shapes"].forEach(function (h) { a[h] && (c[h] = C(b[h]).map(function (b, c) { return u(a[h][c], b) })) }); return c }; c.prototype.addShapes = function () { (this.options.shapes || []).forEach(function (a, b) { a = this.initShape(a, b); u(!0, this.options.shapes[b], a.options) }, this) }; c.prototype.addLabels =
                        function () { (this.options.labels || []).forEach(function (a, b) { a = this.initLabel(a, b); u(!0, this.options.labels[b], a.options) }, this) }; c.prototype.addClipPaths = function () { this.setClipAxes(); this.clipXAxis && this.clipYAxis && (this.clipRect = this.chart.renderer.clipRect(this.getClipBox())) }; c.prototype.setClipAxes = function () {
                            var a = this.chart.xAxis, b = this.chart.yAxis, c = (this.options.labels || []).concat(this.options.shapes || []).reduce(function (c, h) {
                                return [a[h && h.point && h.point.xAxis] || c[0], b[h && h.point && h.point.yAxis] ||
                                    c[1]]
                            }, []); this.clipXAxis = c[0]; this.clipYAxis = c[1]
                        }; c.prototype.getClipBox = function () { if (this.clipXAxis && this.clipYAxis) return { x: this.clipXAxis.left, y: this.clipYAxis.top, width: this.clipXAxis.width, height: this.clipYAxis.height } }; c.prototype.setLabelCollector = function () { var a = this; a.labelCollector = function () { return a.labels.reduce(function (a, b) { b.options.allowOverlap || a.push(b.graphic); return a }, []) }; a.chart.labelCollectors.push(a.labelCollector) }; c.prototype.setOptions = function (a) {
                            this.options = u(this.defaultOptions,
                                a)
                        }; c.prototype.redraw = function (a) { this.linkPoints(); this.graphic || this.render(); this.clipRect && this.clipRect.animate(this.getClipBox()); this.redrawItems(this.shapes, a); this.redrawItems(this.labels, a); f.redraw.call(this, a) }; c.prototype.redrawItems = function (a, b) { for (var c = a.length; c--;)this.redrawItem(a[c], b) }; c.prototype.renderItems = function (a) { for (var b = a.length; b--;)this.renderItem(a[b]) }; c.prototype.render = function () {
                            var a = this.chart.renderer; this.graphic = a.g("annotation").attr({
                                zIndex: this.options.zIndex,
                                visibility: this.options.visible ? "visible" : "hidden"
                            }).add(); this.shapesGroup = a.g("annotation-shapes").add(this.graphic).clip(this.chart.plotBoxClip); this.labelsGroup = a.g("annotation-labels").attr({ translateX: 0, translateY: 0 }).add(this.graphic); this.addClipPaths(); this.clipRect && this.graphic.clip(this.clipRect); this.renderItems(this.shapes); this.renderItems(this.labels); this.addEvents(); f.render.call(this)
                        }; c.prototype.setVisibility = function (a) {
                            var b = this.options; a = y(a, !b.visible); this.graphic.attr("visibility",
                                a ? "visible" : "hidden"); a || this.setControlPointsVisibility(!1); b.visible = a
                        }; c.prototype.setControlPointsVisibility = function (a) { var b = function (b) { b.setControlPointsVisibility(a) }; f.setControlPointsVisibility.call(this, a); this.shapes.forEach(b); this.labels.forEach(b) }; c.prototype.destroy = function () {
                            var b = this.chart, c = function (a) { a.destroy() }; this.labels.forEach(c); this.shapes.forEach(c); this.clipYAxis = this.clipXAxis = null; x(b.labelCollectors, this.labelCollector); a.destroy.call(this); f.destroy.call(this);
                            q(this, b)
                        }; c.prototype.remove = function () { return this.chart.removeAnnotation(this) }; c.prototype.update = function (a, b) { var c = this.chart, h = this.getLabelsAndShapesOptions(this.userOptions, a), e = c.annotations.indexOf(this); a = u(!0, this.userOptions, a); a.labels = h.labels; a.shapes = h.shapes; this.destroy(); this.constructor(c, a); c.options.annotations[e] = a; this.isUpdating = !0; y(b, !0) && c.redraw(); w(this, "afterUpdate"); this.isUpdating = !1 }; c.prototype.initShape = function (a, b) {
                            a = u(this.options.shapeOptions, { controlPointOptions: this.options.controlPointOptions },
                                a); b = new c.shapesMap[a.type](this, a, b); b.itemType = "shape"; this.shapes.push(b); return b
                        }; c.prototype.initLabel = function (a, b) { a = u(this.options.labelOptions, { controlPointOptions: this.options.controlPointOptions }, a); b = new k(this, a, b); b.itemType = "label"; this.labels.push(b); return b }; c.prototype.redrawItem = function (a, b) { a.linkPoints(); a.shouldBeDrawn() ? (a.graphic || this.renderItem(a), a.redraw(y(b, !0) && a.graphic.placed), a.points.length && this.adjustVisibility(a)) : this.destroyItem(a) }; c.prototype.adjustVisibility =
                            function (a) { var b = !1, c = a.graphic; a.points.forEach(function (a) { !1 !== a.series.visible && !1 !== a.visible && (b = !0) }); b ? "hidden" === c.visibility && c.show() : c.hide() }; c.prototype.destroyItem = function (a) { x(this[a.itemType + "s"], a); a.destroy() }; c.prototype.renderItem = function (a) { a.render("label" === a.itemType ? this.labelsGroup : this.shapesGroup) }; c.ControlPoint = b; c.MockPoint = h; c.shapesMap = { rect: l, circle: g, path: r, image: e }; c.types = {}; return c
                }(); u(!0, z.prototype, f, a, u(z.prototype, {
                    nonDOMEvents: ["add", "afterUpdate",
                        "drag", "remove"], defaultOptions: {
                            visible: !0, draggable: "xy", labelOptions: { align: "center", allowOverlap: !1, backgroundColor: "rgba(0, 0, 0, 0.75)", borderColor: "black", borderRadius: 3, borderWidth: 1, className: "", crop: !1, formatter: function () { return n(this.y) ? this.y : "Annotation label" }, overflow: "justify", padding: 5, shadow: !1, shape: "callout", style: { fontSize: "11px", fontWeight: "normal", color: "contrast" }, useHTML: !1, verticalAlign: "bottom", x: 0, y: -16 }, shapeOptions: {
                                stroke: "rgba(0, 0, 0, 0.75)", strokeWidth: 1, fill: "rgba(0, 0, 0, 0.75)",
                                r: 0, snap: 2
                            }, controlPointOptions: { symbol: "circle", width: 10, height: 10, style: { stroke: "black", "stroke-width": 2, fill: "white" }, visible: !1, events: {} }, events: {}, zIndex: 6
                        }
                })); c.extendAnnotation = function (a, b, c, h) { b = b || z; u(!0, a.prototype, b.prototype, c); a.prototype.defaultOptions = u(a.prototype.defaultOptions, h || {}) }; A(d, {
                    initAnnotation: function (a) { a = new (z.types[a.type] || z)(this, a); this.annotations.push(a); return a }, addAnnotation: function (a, b) {
                        a = this.initAnnotation(a); this.options.annotations.push(a.options);
                        y(b, !0) && a.redraw(); return a
                    }, removeAnnotation: function (a) { var b = this.annotations, c = "annotations" === a.coll ? a : v(b, function (b) { return b.options.id === a }); c && (w(c, "remove"), x(this.options.annotations, c.options), x(b, c), c.destroy()) }, drawAnnotations: function () { this.plotBoxClip.attr(this.plotBox); this.annotations.forEach(function (a) { a.redraw() }) }
                }); d.collectionsWithUpdate.push("annotations"); d.collectionsWithInit.annotations = [d.addAnnotation]; d.callbacks.push(function (a) {
                    a.annotations = []; a.options.annotations ||
                        (a.options.annotations = []); a.plotBoxClip = this.renderer.clipRect(this.plotBox); a.controlPointsGroup = a.renderer.g("control-points").attr({ zIndex: 99 }).clip(a.plotBoxClip).add(); a.options.annotations.forEach(function (b, c) { b = a.initAnnotation(b); a.options.annotations[c] = b.options }); a.drawAnnotations(); B(a, "redraw", a.drawAnnotations); B(a, "destroy", function () { a.plotBoxClip.destroy(); a.controlPointsGroup.destroy() })
                }); m(p.prototype, "onContainerMouseDown", function (a) {
                    this.chart.hasDraggedAnnotation || a.apply(this,
                        Array.prototype.slice.call(arguments, 1))
                }); return c.Annotation = z
        }); t(d, "annotations/types/BasicAnnotation.js", [d["annotations/annotations.src.js"], d["annotations/MockPoint.js"], d["parts/Utilities.js"]], function (d, f, l) {
            var g = this && this.__extends || function () {
                var e = function (d, b) { e = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, b) { a.__proto__ = b } || function (a, b) { for (var c in b) b.hasOwnProperty(c) && (a[c] = b[c]) }; return e(d, b) }; return function (d, b) {
                    function a() { this.constructor = d } e(d, b);
                    d.prototype = null === b ? Object.create(b) : (a.prototype = b.prototype, new a)
                }
            }(); l = l.merge; var r = function (e) {
                function d(b, a) { return e.call(this, b, a) || this } g(d, e); d.prototype.addControlPoints = function () { var b = this.options, a = d.basicControlPoints, c = b.langKey; (b.labels || b.shapes).forEach(function (b) { c && (b.controlPoints = a[c]) }) }; d.basicControlPoints = {
                    label: [{
                        symbol: "triangle-down", positioner: function (b) {
                            if (!b.graphic.placed) return { x: 0, y: -9E7 }; b = f.pointToPixels(b.points[0]); return {
                                x: b.x - this.graphic.width / 2, y: b.y -
                                    this.graphic.height / 2
                            }
                        }, events: { drag: function (b, a) { b = this.mouseMoveToTranslation(b); a.translatePoint(b.x, b.y); a.annotation.userOptions.labels[0].point = a.options.point; a.redraw(!1) } }
                    }, { symbol: "square", positioner: function (b) { return b.graphic.placed ? { x: b.graphic.alignAttr.x - this.graphic.width / 2, y: b.graphic.alignAttr.y - this.graphic.height / 2 } : { x: 0, y: -9E7 } }, events: { drag: function (b, a) { b = this.mouseMoveToTranslation(b); a.translate(b.x, b.y); a.annotation.userOptions.labels[0].point = a.options.point; a.redraw(!1) } } }],
                    rectangle: [{ positioner: function (b) { b = f.pointToPixels(b.points[2]); return { x: b.x - 4, y: b.y - 4 } }, events: { drag: function (b, a) { var c = a.annotation, h = this.chart.pointer.getCoordinates(b); b = h.xAxis[0].value; h = h.yAxis[0].value; var e = a.options.points; e[1].x = b; e[2].x = b; e[2].y = h; e[3].y = h; c.userOptions.shapes[0].points = a.options.points; c.redraw(!1) } } }], circle: [{
                        positioner: function (b) {
                            var a = f.pointToPixels(b.points[0]); b = b.options.r; return {
                                x: a.x + b * Math.cos(Math.PI / 4) - this.graphic.width / 2, y: a.y + b * Math.sin(Math.PI / 4) -
                                    this.graphic.height / 2
                            }
                        }, events: { drag: function (b, a) { var c = a.annotation; b = this.mouseMoveToTranslation(b); a.setRadius(Math.max(a.options.r + b.y / Math.sin(Math.PI / 4), 5)); c.userOptions.shapes[0].r = a.options.r; c.userOptions.shapes[0].point = a.options.point; a.redraw(!1) } }
                    }]
                }; return d
            }(d); r.prototype.defaultOptions = l(d.prototype.defaultOptions, {}); return d.types.basicAnnotation = r
        }); t(d, "annotations/types/CrookedLine.js", [d["annotations/annotations.src.js"], d["annotations/ControlPoint.js"], d["annotations/MockPoint.js"],
        d["parts/Utilities.js"]], function (d, f, l, g) {
            var r = this && this.__extends || function () { var e = function (b, a) { e = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, b) { a.__proto__ = b } || function (a, b) { for (var c in b) b.hasOwnProperty(c) && (a[c] = b[c]) }; return e(b, a) }; return function (b, a) { function c() { this.constructor = b } e(b, a); b.prototype = null === a ? Object.create(a) : (c.prototype = a.prototype, new c) } }(), e = g.merge; g = function (d) {
                function b(a, b) { return d.call(this, a, b) || this } r(b, d); b.prototype.setClipAxes =
                    function () { this.clipXAxis = this.chart.xAxis[this.options.typeOptions.xAxis]; this.clipYAxis = this.chart.yAxis[this.options.typeOptions.yAxis] }; b.prototype.getPointsOptions = function () { var a = this.options.typeOptions; return (a.points || []).map(function (b) { b.xAxis = a.xAxis; b.yAxis = a.yAxis; return b }) }; b.prototype.getControlPointsOptions = function () { return this.getPointsOptions() }; b.prototype.addControlPoints = function () {
                        this.getControlPointsOptions().forEach(function (a, b) {
                            b = new f(this.chart, this, e(this.options.controlPointOptions,
                                a.controlPoint), b); this.controlPoints.push(b); a.controlPoint = b.options
                        }, this)
                    }; b.prototype.addShapes = function () { var a = this.options.typeOptions, b = this.initShape(e(a.line, { type: "path", points: this.points.map(function (a, b) { return function (a) { return a.annotation.points[b] } }) }), !1); a.line = b.options }; return b
            }(d); g.prototype.defaultOptions = e(d.prototype.defaultOptions, {
                typeOptions: { xAxis: 0, yAxis: 0, line: { fill: "none" } }, controlPointOptions: {
                    positioner: function (e) {
                        var b = this.graphic; e = l.pointToPixels(e.points[this.index]);
                        return { x: e.x - b.width / 2, y: e.y - b.height / 2 }
                    }, events: { drag: function (e, b) { b.chart.isInsidePlot(e.chartX - b.chart.plotLeft, e.chartY - b.chart.plotTop) && (e = this.mouseMoveToTranslation(e), b.translatePoint(e.x, e.y, this.index), b.options.typeOptions.points[this.index].x = b.points[this.index].x, b.options.typeOptions.points[this.index].y = b.points[this.index].y, b.redraw(!1)) } }
                }
            }); return d.types.crookedLine = g
        }); t(d, "annotations/types/ElliottWave.js", [d["annotations/annotations.src.js"], d["annotations/types/CrookedLine.js"],
        d["parts/Utilities.js"]], function (d, f, l) {
            var g = this && this.__extends || function () { var e = function (d, b) { e = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, b) { a.__proto__ = b } || function (a, b) { for (var c in b) b.hasOwnProperty(c) && (a[c] = b[c]) }; return e(d, b) }; return function (d, b) { function a() { this.constructor = d } e(d, b); d.prototype = null === b ? Object.create(b) : (a.prototype = b.prototype, new a) } }(), r = l.merge; l = function (e) {
                function d(b, a) { return e.call(this, b, a) || this } g(d, e); d.prototype.addLabels = function () {
                    this.getPointsOptions().forEach(function (b,
                        a) { var c = this.initLabel(r(b.label, { text: this.options.typeOptions.labels[a], point: function (b) { return b.annotation.points[a] } }), !1); b.label = c.options }, this)
                }; return d
            }(f); l.prototype.defaultOptions = r(f.prototype.defaultOptions, { typeOptions: { labels: "(0) (A) (B) (C) (D) (E)".split(" "), line: { strokeWidth: 1 } }, labelOptions: { align: "center", allowOverlap: !0, crop: !0, overflow: "none", type: "rect", backgroundColor: "none", borderWidth: 0, y: -5 } }); return d.types.elliottWave = l
        }); t(d, "annotations/types/Tunnel.js", [d["annotations/annotations.src.js"],
        d["annotations/ControlPoint.js"], d["annotations/types/CrookedLine.js"], d["annotations/MockPoint.js"], d["parts/Utilities.js"]], function (d, f, l, g, r) {
            var e = this && this.__extends || function () { var b = function (a, c) { b = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, b) { a.__proto__ = b } || function (a, b) { for (var c in b) b.hasOwnProperty(c) && (a[c] = b[c]) }; return b(a, c) }; return function (a, c) { function e() { this.constructor = a } b(a, c); a.prototype = null === c ? Object.create(c) : (e.prototype = c.prototype, new e) } }(),
            k = r.merge; r = function (b) {
                function a(a, e) { return b.call(this, a, e) || this } e(a, b); a.prototype.getPointsOptions = function () { var a = l.prototype.getPointsOptions.call(this); a[2] = this.heightPointOptions(a[1]); a[3] = this.heightPointOptions(a[0]); return a }; a.prototype.getControlPointsOptions = function () { return this.getPointsOptions().slice(0, 2) }; a.prototype.heightPointOptions = function (a) { a = k(a); a.y += this.options.typeOptions.height; return a }; a.prototype.addControlPoints = function () {
                    l.prototype.addControlPoints.call(this);
                    var a = this.options, b = a.typeOptions; a = new f(this.chart, this, k(a.controlPointOptions, b.heightControlPoint), 2); this.controlPoints.push(a); b.heightControlPoint = a.options
                }; a.prototype.addShapes = function () { this.addLine(); this.addBackground() }; a.prototype.addLine = function () {
                    var a = this.initShape(k(this.options.typeOptions.line, { type: "path", points: [this.points[0], this.points[1], function (a) { a = g.pointToOptions(a.annotation.points[2]); a.command = "M"; return a }, this.points[3]] }), !1); this.options.typeOptions.line =
                        a.options
                }; a.prototype.addBackground = function () { var a = this.initShape(k(this.options.typeOptions.background, { type: "path", points: this.points.slice() })); this.options.typeOptions.background = a.options }; a.prototype.translateSide = function (a, b, e) { e = Number(e); var c = 0 === e ? 3 : 2; this.translatePoint(a, b, e); this.translatePoint(a, b, c) }; a.prototype.translateHeight = function (a) { this.translatePoint(0, a, 2); this.translatePoint(0, a, 3); this.options.typeOptions.height = this.points[3].y - this.points[0].y }; return a
            }(l); r.prototype.defaultOptions =
                k(l.prototype.defaultOptions, {
                    typeOptions: {
                        xAxis: 0, yAxis: 0, background: { fill: "rgba(130, 170, 255, 0.4)", strokeWidth: 0 }, line: { strokeWidth: 1 }, height: -2, heightControlPoint: {
                            positioner: function (b) { var a = g.pointToPixels(b.points[2]); b = g.pointToPixels(b.points[3]); var c = (a.x + b.x) / 2; return { x: c - this.graphic.width / 2, y: (b.y - a.y) / (b.x - a.x) * (c - a.x) + a.y - this.graphic.height / 2 } }, events: {
                                drag: function (b, a) {
                                    a.chart.isInsidePlot(b.chartX - a.chart.plotLeft, b.chartY - a.chart.plotTop) && (a.translateHeight(this.mouseMoveToTranslation(b).y),
                                        a.redraw(!1))
                                }
                            }
                        }
                    }, controlPointOptions: { events: { drag: function (b, a) { a.chart.isInsidePlot(b.chartX - a.chart.plotLeft, b.chartY - a.chart.plotTop) && (b = this.mouseMoveToTranslation(b), a.translateSide(b.x, b.y, this.index), a.redraw(!1)) } } }
                }); return d.types.tunnel = r
        }); t(d, "annotations/types/InfinityLine.js", [d["annotations/annotations.src.js"], d["annotations/types/CrookedLine.js"], d["annotations/MockPoint.js"], d["parts/Utilities.js"]], function (d, f, l, g) {
            var r = this && this.__extends || function () {
                var e = function (b, a) {
                    e =
                    Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, b) { a.__proto__ = b } || function (a, b) { for (var c in b) b.hasOwnProperty(c) && (a[c] = b[c]) }; return e(b, a)
                }; return function (b, a) { function c() { this.constructor = b } e(b, a); b.prototype = null === a ? Object.create(a) : (c.prototype = a.prototype, new c) }
            }(), e = g.merge; g = function (d) {
                function b(a, b) { return d.call(this, a, b) || this } r(b, d); b.edgePoint = function (a, c) {
                    return function (e) {
                        e = e.annotation; var d = e.points, h = e.options.typeOptions.type; "horizontalLine" === h ? d =
                            [d[0], new l(e.chart, d[0].target, { x: d[0].x + 1, y: d[0].y, xAxis: d[0].options.xAxis, yAxis: d[0].options.yAxis })] : "verticalLine" === h && (d = [d[0], new l(e.chart, d[0].target, { x: d[0].x, y: d[0].y + 1, xAxis: d[0].options.xAxis, yAxis: d[0].options.yAxis })]); return b.findEdgePoint(d[a], d[c])
                    }
                }; b.findEdgeCoordinate = function (a, b, e, d) { var c = "x" === e ? "y" : "x"; return (b[e] - a[e]) * (d - a[c]) / (b[c] - a[c]) + a[e] }; b.findEdgePoint = function (a, c) {
                    var e = a.series.xAxis, d = c.series.yAxis, m = l.pointToPixels(a), f = l.pointToPixels(c), n = f.x - m.x, q = f.y -
                        m.y; c = e.left; var k = c + e.width; e = d.top; d = e + d.height; var g = 0 > n ? c : k, v = 0 > q ? e : d; k = { x: 0 === n ? m.x : g, y: 0 === q ? m.y : v }; 0 !== n && 0 !== q && (n = b.findEdgeCoordinate(m, f, "y", g), m = b.findEdgeCoordinate(m, f, "x", v), n >= e && n <= d ? (k.x = g, k.y = n) : (k.x = m, k.y = v)); k.x -= c; k.y -= e; a.series.chart.inverted && (a = k.x, k.x = k.y, k.y = a); return k
                }; b.prototype.addShapes = function () {
                    var a = this.options.typeOptions, c = [this.points[0], b.endEdgePoint]; a.type.match(/Line/g) && (c[0] = b.startEdgePoint); c = this.initShape(e(a.line, { type: "path", points: c }), !1); a.line =
                        c.options
                }; b.endEdgePoint = b.edgePoint(0, 1); b.startEdgePoint = b.edgePoint(1, 0); return b
            }(f); g.prototype.defaultOptions = e(f.prototype.defaultOptions, {}); return d.types.infinityLine = g
        }); t(d, "annotations/types/Fibonacci.js", [d["annotations/annotations.src.js"], d["annotations/MockPoint.js"], d["annotations/types/Tunnel.js"], d["parts/Utilities.js"]], function (d, f, l, g) {
            var r = this && this.__extends || function () {
                var b = function (a, c) {
                    b = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, b) {
                        a.__proto__ =
                        b
                    } || function (a, b) { for (var c in b) b.hasOwnProperty(c) && (a[c] = b[c]) }; return b(a, c)
                }; return function (a, c) { function e() { this.constructor = a } b(a, c); a.prototype = null === c ? Object.create(c) : (e.prototype = c.prototype, new e) }
            }(), e = g.merge, k = function (b, a) {
                return function () {
                    var c = this.annotation, e = this.anchor(c.startRetracements[b]).absolutePosition, d = this.anchor(c.endRetracements[b]).absolutePosition; e = [["M", Math.round(e.x), Math.round(e.y)], ["L", Math.round(d.x), Math.round(d.y)]]; a && (d = this.anchor(c.endRetracements[b -
                        1]).absolutePosition, c = this.anchor(c.startRetracements[b - 1]).absolutePosition, e.push(["L", Math.round(d.x), Math.round(d.y)], ["L", Math.round(c.x), Math.round(c.y)])); return e
                }
            }; g = function (b) {
                function a(a, e) { return b.call(this, a, e) || this } r(a, b); a.prototype.linkPoints = function () { b.prototype.linkPoints.call(this); this.linkRetracementsPoints() }; a.prototype.linkRetracementsPoints = function () {
                    var b = this.points, e = b[0].y - b[3].y, d = b[1].y - b[2].y, m = b[0].x, f = b[1].x; a.levels.forEach(function (a, c) {
                        var h = b[0].y - e * a;
                        a = b[1].y - d * a; this.startRetracements = this.startRetracements || []; this.endRetracements = this.endRetracements || []; this.linkRetracementPoint(c, m, h, this.startRetracements); this.linkRetracementPoint(c, f, a, this.endRetracements)
                    }, this)
                }; a.prototype.linkRetracementPoint = function (a, b, e, d) { var c = d[a], h = this.options.typeOptions; c ? (c.options.x = b, c.options.y = e, c.refresh()) : d[a] = new f(this.chart, this, { x: b, y: e, xAxis: h.xAxis, yAxis: h.yAxis }) }; a.prototype.addShapes = function () {
                    a.levels.forEach(function (a, b) {
                        this.initShape({
                            type: "path",
                            d: k(b)
                        }, !1); 0 < b && this.initShape({ type: "path", fill: this.options.typeOptions.backgroundColors[b - 1], strokeWidth: 0, d: k(b, !0) })
                    }, this)
                }; a.prototype.addLabels = function () { a.levels.forEach(function (a, b) { var c = this.options.typeOptions; a = this.initLabel(e(c.labels[b], { point: function (a) { return f.pointToOptions(a.annotation.startRetracements[b]) }, text: a.toString() })); c.labels[b] = a.options }, this) }; a.levels = [0, .236, .382, .5, .618, .786, 1]; return a
            }(l); g.prototype.defaultOptions = e(l.prototype.defaultOptions, {
                typeOptions: {
                    height: 2,
                    backgroundColors: "rgba(130, 170, 255, 0.4);rgba(139, 191, 216, 0.4);rgba(150, 216, 192, 0.4);rgba(156, 229, 161, 0.4);rgba(162, 241, 130, 0.4);rgba(169, 255, 101, 0.4)".split(";"), lineColor: "grey", lineColors: [], labels: []
                }, labelOptions: { allowOverlap: !0, align: "right", backgroundColor: "none", borderWidth: 0, crop: !1, overflow: "none", shape: "rect", style: { color: "grey" }, verticalAlign: "middle", y: 0 }
            }); return d.types.fibonacci = g
        }); t(d, "annotations/types/Pitchfork.js", [d["annotations/annotations.src.js"], d["annotations/types/InfinityLine.js"],
        d["annotations/MockPoint.js"], d["parts/Utilities.js"]], function (d, f, l, g) {
            var r = this && this.__extends || function () { var e = function (b, a) { e = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, b) { a.__proto__ = b } || function (a, b) { for (var c in b) b.hasOwnProperty(c) && (a[c] = b[c]) }; return e(b, a) }; return function (b, a) { function c() { this.constructor = b } e(b, a); b.prototype = null === a ? Object.create(a) : (c.prototype = a.prototype, new c) } }(), e = g.merge; g = function (d) {
                function b(a, b) { return d.call(this, a, b) || this }
                r(b, d); b.outerLineEdgePoint = function (a) { return function (c) { var e = c.annotation, d = e.points; return b.findEdgePoint(d[a], d[0], new l(e.chart, c, e.midPointOptions())) } }; b.findEdgePoint = function (a, b, e) { b = Math.atan2(e.plotY - b.plotY, e.plotX - b.plotX); return { x: a.plotX + 1E7 * Math.cos(b), y: a.plotY + 1E7 * Math.sin(b) } }; b.middleLineEdgePoint = function (a) { var b = a.annotation; return f.findEdgePoint(b.points[0], new l(b.chart, a, b.midPointOptions())) }; b.prototype.midPointOptions = function () {
                    var a = this.points; return {
                        x: (a[1].x +
                            a[2].x) / 2, y: (a[1].y + a[2].y) / 2, xAxis: a[0].series.xAxis, yAxis: a[0].series.yAxis
                    }
                }; b.prototype.addShapes = function () { this.addLines(); this.addBackgrounds() }; b.prototype.addLines = function () { this.initShape({ type: "path", points: [this.points[0], b.middleLineEdgePoint] }, !1); this.initShape({ type: "path", points: [this.points[1], b.topLineEdgePoint] }, !1); this.initShape({ type: "path", points: [this.points[2], b.bottomLineEdgePoint] }, !1) }; b.prototype.addBackgrounds = function () {
                    var a = this.shapes, b = this.options.typeOptions,
                    d = this.initShape(e(b.innerBackground, { type: "path", points: [function (a) { var b = a.annotation; a = b.points; b = b.midPointOptions(); return { x: (a[1].x + b.x) / 2, y: (a[1].y + b.y) / 2, xAxis: b.xAxis, yAxis: b.yAxis } }, a[1].points[1], a[2].points[1], function (a) { var b = a.annotation; a = b.points; b = b.midPointOptions(); return { x: (b.x + a[2].x) / 2, y: (b.y + a[2].y) / 2, xAxis: b.xAxis, yAxis: b.yAxis } }] })); a = this.initShape(e(b.outerBackground, { type: "path", points: [this.points[1], a[1].points[1], a[2].points[1], this.points[2]] })); b.innerBackground =
                        d.options; b.outerBackground = a.options
                }; b.topLineEdgePoint = b.outerLineEdgePoint(1); b.bottomLineEdgePoint = b.outerLineEdgePoint(0); return b
            }(f); g.prototype.defaultOptions = e(f.prototype.defaultOptions, { typeOptions: { innerBackground: { fill: "rgba(130, 170, 255, 0.4)", strokeWidth: 0 }, outerBackground: { fill: "rgba(156, 229, 161, 0.4)", strokeWidth: 0 } } }); return d.types.pitchfork = g
        }); t(d, "annotations/types/VerticalLine.js", [d["annotations/annotations.src.js"], d["parts/Globals.js"], d["annotations/MockPoint.js"], d["parts/Utilities.js"]],
            function (d, f, l, g) {
                var r = g.merge, e = function () { d.apply(this, arguments) }; e.connectorFirstPoint = function (e) { e = e.annotation; var b = e.points[0], a = l.pointToPixels(b, !0), c = a.y, d = e.options.typeOptions.label.offset; e.chart.inverted && (c = a.x); return { x: b.x, xAxis: b.series.xAxis, y: c + d } }; e.connectorSecondPoint = function (e) { var b = e.annotation; e = b.options.typeOptions; var a = b.points[0], c = e.yOffset; b = l.pointToPixels(a, !0)[b.chart.inverted ? "x" : "y"]; 0 > e.label.offset && (c *= -1); return { x: a.x, xAxis: a.series.xAxis, y: b + c } }; f.extendAnnotation(e,
                    null, {
                        getPointsOptions: function () { return [this.options.typeOptions.point] }, addShapes: function () { var d = this.options.typeOptions, b = this.initShape(r(d.connector, { type: "path", points: [e.connectorFirstPoint, e.connectorSecondPoint] }), !1); d.connector = b.options }, addLabels: function () {
                            var e = this.options.typeOptions, b = e.label, a = 0, c = b.offset, d = 0 > b.offset ? "bottom" : "top", f = "center"; this.chart.inverted && (a = b.offset, c = 0, d = "middle", f = 0 > b.offset ? "right" : "left"); b = this.initLabel(r(b, { verticalAlign: d, align: f, x: a, y: c }));
                            e.label = b.options
                        }
                }, { typeOptions: { yOffset: 10, label: { offset: -40, point: function (e) { return e.annotation.points[0] }, allowOverlap: !0, backgroundColor: "none", borderWidth: 0, crop: !0, overflow: "none", shape: "rect", text: "{y:.2f}" }, connector: { strokeWidth: 1, markerEnd: "arrow" } } }); return d.types.verticalLine = e
            }); t(d, "annotations/types/Measure.js", [d["annotations/annotations.src.js"], d["annotations/ControlPoint.js"], d["parts/Globals.js"], d["parts/Utilities.js"]], function (d, f, l, g) {
                var r = g.extend, e = g.isNumber, k = g.merge;
                g = function () { d.apply(this, arguments) }; d.types.measure = g; l.extendAnnotation(g, null, {
                    init: function () { d.prototype.init.apply(this, arguments); this.resizeY = this.resizeX = this.offsetY = this.offsetX = 0; this.calculations.init.call(this); this.addValues(); this.addShapes() }, setClipAxes: function () { this.clipXAxis = this.chart.xAxis[this.options.typeOptions.xAxis]; this.clipYAxis = this.chart.yAxis[this.options.typeOptions.yAxis] }, pointsOptions: function () { return this.options.points }, shapePointsOptions: function () {
                        var b =
                            this.options.typeOptions, a = b.xAxis; b = b.yAxis; return [{ x: this.xAxisMin, y: this.yAxisMin, xAxis: a, yAxis: b }, { x: this.xAxisMax, y: this.yAxisMin, xAxis: a, yAxis: b }, { x: this.xAxisMax, y: this.yAxisMax, xAxis: a, yAxis: b }, { x: this.xAxisMin, y: this.yAxisMax, xAxis: a, yAxis: b }]
                    }, addControlPoints: function () { var b = this.options.typeOptions.selectType; var a = new f(this.chart, this, this.options.controlPointOptions, 0); this.controlPoints.push(a); "xy" !== b && (a = new f(this.chart, this, this.options.controlPointOptions, 1), this.controlPoints.push(a)) },
                    addValues: function (b) {
                        var a = this.options.typeOptions, c = a.label.formatter; this.calculations.recalculate.call(this, b); a.label.enabled && (0 < this.labels.length ? this.labels[0].text = c && c.call(this) || this.calculations.defaultFormatter.call(this) : this.initLabel(r({
                            shape: "rect", backgroundColor: "none", color: "black", borderWidth: 0, dashStyle: "dash", overflow: "none", align: "left", vertical: "top", crop: !0, point: function (b) {
                                b = b.annotation; var c = b.chart, e = c.inverted, d = c.yAxis[a.yAxis], h = c.plotTop, f = c.plotLeft; return {
                                    x: (e ?
                                        h : 10) + c.xAxis[a.xAxis].toPixels(b.xAxisMin, !e), y: (e ? -f + 10 : h) + d.toPixels(b.yAxisMin)
                                }
                            }, text: c && c.call(this) || this.calculations.defaultFormatter.call(this)
                        }, a.label)))
                    }, addShapes: function () { this.addCrosshairs(); this.addBackground() }, addBackground: function () { "undefined" !== typeof this.shapePointsOptions()[0].x && this.initShape(r({ type: "path", points: this.shapePointsOptions() }, this.options.typeOptions.background), !1) }, addCrosshairs: function () {
                        var b = this.chart, a = this.options.typeOptions, c = this.options.typeOptions.point,
                        e = b.xAxis[a.xAxis], d = b.yAxis[a.yAxis], f = b.inverted; b = e.toPixels(this.xAxisMin); e = e.toPixels(this.xAxisMax); var g = d.toPixels(this.yAxisMin), n = d.toPixels(this.yAxisMax), q = { point: c, type: "path" }; c = []; d = []; f && (f = b, b = g, g = f, f = e, e = n, n = f); a.crosshairX.enabled && (c = [["M", b, g + (n - g) / 2], ["L", e, g + (n - g) / 2]]); a.crosshairY.enabled && (d = [["M", b + (e - b) / 2, g], ["L", b + (e - b) / 2, n]]); 0 < this.shapes.length ? (this.shapes[0].options.d = c, this.shapes[1].options.d = d) : (b = k(q, a.crosshairX), a = k(q, a.crosshairY), this.initShape(r({ d: c }, b),
                            !1), this.initShape(r({ d: d }, a), !1))
                    }, onDrag: function (b) { var a = this.mouseMoveToTranslation(b), c = this.options.typeOptions.selectType; b = "y" === c ? 0 : a.x; a = "x" === c ? 0 : a.y; this.translate(b, a); this.offsetX += b; this.offsetY += a; this.redraw(!1, !1, !0) }, resize: function (b, a, c, e) {
                        var d = this.shapes[2]; "x" === e ? 0 === c ? (d.translatePoint(b, 0, 0), d.translatePoint(b, a, 3)) : (d.translatePoint(b, 0, 1), d.translatePoint(b, a, 2)) : "y" === e ? 0 === c ? (d.translatePoint(0, a, 0), d.translatePoint(0, a, 1)) : (d.translatePoint(0, a, 2), d.translatePoint(0,
                            a, 3)) : (d.translatePoint(b, 0, 1), d.translatePoint(b, a, 2), d.translatePoint(0, a, 3)); this.calculations.updateStartPoints.call(this, !1, !0, c, b, a); this.options.typeOptions.background.height = Math.abs(this.startYMax - this.startYMin); this.options.typeOptions.background.width = Math.abs(this.startXMax - this.startXMin)
                    }, redraw: function (b, a, c) {
                        this.linkPoints(); this.graphic || this.render(); c && this.calculations.updateStartPoints.call(this, !0, !1); this.clipRect && this.clipRect.animate(this.getClipBox()); this.addValues(a);
                        this.addCrosshairs(); this.redrawItems(this.shapes, b); this.redrawItems(this.labels, b); this.controlPoints.forEach(function (a) { a.redraw() })
                    }, translate: function (b, a) { this.shapes.forEach(function (c) { c.translate(b, a) }); this.options.typeOptions.point.x = this.startXMin; this.options.typeOptions.point.y = this.startYMin }, calculations: {
                        init: function () {
                            var b = this.options.typeOptions, a = this.chart, c = this.calculations.getPointPos, d = a.inverted, f = a.xAxis[b.xAxis], g = a.yAxis[b.yAxis], k = b.background, n = d ? k.height : k.width;
                            k = d ? k.width : k.height; var q = b.selectType, l = d ? a.plotLeft : a.plotTop; a = d ? a.plotTop : a.plotLeft; this.startXMin = b.point.x; this.startYMin = b.point.y; e(n) ? this.startXMax = this.startXMin + n : this.startXMax = c(f, this.startXMin, parseFloat(n)); e(k) ? this.startYMax = this.startYMin - k : this.startYMax = c(g, this.startYMin, parseFloat(k)); "x" === q ? (this.startYMin = g.toValue(l), this.startYMax = g.toValue(l + g.len)) : "y" === q && (this.startXMin = f.toValue(a), this.startXMax = f.toValue(a + f.len))
                        }, recalculate: function (b) {
                            var a = this.calculations,
                            c = this.options.typeOptions, e = this.chart.xAxis[c.xAxis]; c = this.chart.yAxis[c.yAxis]; var d = this.calculations.getPointPos, f = this.offsetX, g = this.offsetY; this.xAxisMin = d(e, this.startXMin, f); this.xAxisMax = d(e, this.startXMax, f); this.yAxisMin = d(c, this.startYMin, g); this.yAxisMax = d(c, this.startYMax, g); this.min = a.min.call(this); this.max = a.max.call(this); this.average = a.average.call(this); this.bins = a.bins.call(this); b && this.resize(0, 0)
                        }, getPointPos: function (b, a, c) { return b.toValue(b.toPixels(a) + c) }, updateStartPoints: function (b,
                            a, c, e, d) {
                                var f = this.options.typeOptions, h = f.selectType, n = this.chart.xAxis[f.xAxis]; f = this.chart.yAxis[f.yAxis]; var q = this.calculations.getPointPos, g = this.startXMin, k = this.startXMax, l = this.startYMin, w = this.startYMax, u = this.offsetX, y = this.offsetY; a && ("x" === h ? 0 === c ? this.startXMin = q(n, g, e) : this.startXMax = q(n, k, e) : "y" === h ? 0 === c ? this.startYMin = q(f, l, d) : this.startYMax = q(f, w, d) : (this.startXMax = q(n, k, e), this.startYMax = q(f, w, d))); b && (this.startXMin = q(n, g, u), this.startXMax = q(n, k, u), this.startYMin = q(f, l, y), this.startYMax =
                                    q(f, w, y), this.offsetY = this.offsetX = 0)
                        }, defaultFormatter: function () { return "Min: " + this.min + "<br>Max: " + this.max + "<br>Average: " + this.average + "<br>Bins: " + this.bins }, getExtremes: function (b, a, c, e) { return { xAxisMin: Math.min(a, b), xAxisMax: Math.max(a, b), yAxisMin: Math.min(e, c), yAxisMax: Math.max(e, c) } }, min: function () {
                            var b = Infinity, a = this.chart.series, c = this.calculations.getExtremes(this.xAxisMin, this.xAxisMax, this.yAxisMin, this.yAxisMax), e = !1; a.forEach(function (a) {
                                a.visible && "highcharts-navigator-series" !==
                                    a.options.id && a.points.forEach(function (a) { !a.isNull && a.y < b && a.x > c.xAxisMin && a.x <= c.xAxisMax && a.y > c.yAxisMin && a.y <= c.yAxisMax && (b = a.y, e = !0) })
                            }); e || (b = ""); return b
                        }, max: function () {
                            var b = -Infinity, a = this.chart.series, c = this.calculations.getExtremes(this.xAxisMin, this.xAxisMax, this.yAxisMin, this.yAxisMax), e = !1; a.forEach(function (a) {
                                a.visible && "highcharts-navigator-series" !== a.options.id && a.points.forEach(function (a) {
                                    !a.isNull && a.y > b && a.x > c.xAxisMin && a.x <= c.xAxisMax && a.y > c.yAxisMin && a.y <= c.yAxisMax &&
                                    (b = a.y, e = !0)
                                })
                            }); e || (b = ""); return b
                        }, average: function () { var b = ""; "" !== this.max && "" !== this.min && (b = (this.max + this.min) / 2); return b }, bins: function () { var b = 0, a = this.chart.series, c = this.calculations.getExtremes(this.xAxisMin, this.xAxisMax, this.yAxisMin, this.yAxisMax), e = !1; a.forEach(function (a) { a.visible && "highcharts-navigator-series" !== a.options.id && a.points.forEach(function (a) { !a.isNull && a.x > c.xAxisMin && a.x <= c.xAxisMax && a.y > c.yAxisMin && a.y <= c.yAxisMax && (b++, e = !0) }) }); e || (b = ""); return b }
                    }
                }, {
                    typeOptions: {
                        selectType: "xy",
                        xAxis: 0, yAxis: 0, background: { fill: "rgba(130, 170, 255, 0.4)", strokeWidth: 0, stroke: void 0 }, crosshairX: { enabled: !0, zIndex: 6, dashStyle: "Dash", markerEnd: "arrow" }, crosshairY: { enabled: !0, zIndex: 6, dashStyle: "Dash", markerEnd: "arrow" }, label: { enabled: !0, style: { fontSize: "11px", color: "#666666" }, formatter: void 0 }
                    }, controlPointOptions: {
                        positioner: function (b) {
                            var a = this.index, c = b.chart, e = b.options, d = e.typeOptions, f = d.selectType; e = e.controlPointOptions; var g = c.inverted, n = c.xAxis[d.xAxis]; c = c.yAxis[d.yAxis]; d = b.xAxisMax;
                            var q = b.yAxisMax, k = b.calculations.getExtremes(b.xAxisMin, b.xAxisMax, b.yAxisMin, b.yAxisMax); "x" === f && (q = (k.yAxisMax - k.yAxisMin) / 2, 0 === a && (d = b.xAxisMin)); "y" === f && (d = k.xAxisMin + (k.xAxisMax - k.xAxisMin) / 2, 0 === a && (q = b.yAxisMin)); g ? (b = c.toPixels(q), a = n.toPixels(d)) : (b = n.toPixels(d), a = c.toPixels(q)); return { x: b - e.width / 2, y: a - e.height / 2 }
                        }, events: {
                            drag: function (b, a) {
                                var c = this.mouseMoveToTranslation(b); b = a.options.typeOptions.selectType; var e = "y" === b ? 0 : c.x; c = "x" === b ? 0 : c.y; a.resize(e, c, this.index, b); a.resizeX +=
                                    e; a.resizeY += c; a.redraw(!1, !0)
                            }
                        }
                    }
                }); return d.types.measure = g
            }); t(d, "mixins/navigation.js", [], function () { return { initUpdate: function (d) { d.navigation || (d.navigation = { updates: [], update: function (d, l) { this.updates.forEach(function (f) { f.update.call(f.context, d, l) }) } }) }, addUpdate: function (d, f) { f.navigation || this.initUpdate(f); f.navigation.updates.push({ update: d, context: f }) } } }); t(d, "annotations/navigationBindings.js", [d["annotations/annotations.src.js"], d["mixins/navigation.js"], d["parts/Globals.js"], d["parts/Utilities.js"]],
                function (d, f, l, g) {
                    function r(b) {
                        var c = b.prototype.defaultOptions.events && b.prototype.defaultOptions.events.click; t(!0, b.prototype.defaultOptions.events, {
                            click: function (b) {
                                var e = this, d = e.chart.navigationBindings, f = d.activeAnnotation; c && c.call(e, b); f !== e ? (d.deselectAnnotation(), d.activeAnnotation = e, e.setControlPointsVisibility(!0), a(d, "showPopup", {
                                    annotation: e, formType: "annotation-toolbar", options: d.annotationToFields(e), onSubmit: function (a) {
                                        var b = {}; "remove" === a.actionType ? (d.activeAnnotation = !1, d.chart.removeAnnotation(e)) :
                                            (d.fieldsToOptions(a.fields, b), d.deselectAnnotation(), a = b.typeOptions, "measure" === e.options.type && (a.crosshairY.enabled = 0 !== a.crosshairY.strokeWidth, a.crosshairX.enabled = 0 !== a.crosshairX.strokeWidth), e.update(b))
                                    }
                                })) : (d.deselectAnnotation(), a(d, "closePopup")); b.activeAnnotation = !0
                            }
                        })
                    } var e = g.addEvent, k = g.attr, b = g.format, a = g.fireEvent, c = g.isArray, h = g.isFunction, p = g.isNumber, m = g.isObject, t = g.merge, n = g.objectEach, q = g.pick; g = g.setOptions; var x = l.doc, A = l.win, v = function () {
                        function d(a, b) {
                            this.selectedButton =
                            this.boundClassNames = void 0; this.chart = a; this.options = b; this.eventsToUnbind = []; this.container = x.getElementsByClassName(this.options.bindingsClassName || "")
                        } d.prototype.initEvents = function () {
                            var a = this, b = a.chart, c = a.container, d = a.options; a.boundClassNames = {}; n(d.bindings || {}, function (b) { a.boundClassNames[b.className] = b });[].forEach.call(c, function (b) { a.eventsToUnbind.push(e(b, "click", function (c) { var d = a.getButtonEvents(b, c); d && a.bindingsButtonClick(d.button, d.events, c) })) }); n(d.events || {}, function (b,
                                c) { h(b) && a.eventsToUnbind.push(e(a, c, b)) }); a.eventsToUnbind.push(e(b.container, "click", function (c) { !b.cancelClick && b.isInsidePlot(c.chartX - b.plotLeft, c.chartY - b.plotTop) && a.bindingsChartClick(this, c) })); a.eventsToUnbind.push(e(b.container, l.isTouchDevice ? "touchmove" : "mousemove", function (b) { a.bindingsContainerMouseMove(this, b) }))
                        }; d.prototype.initUpdate = function () { var a = this; f.addUpdate(function (b) { a.update(b) }, this.chart) }; d.prototype.bindingsButtonClick = function (b, c, d) {
                            var e = this.chart; this.selectedButtonElement &&
                                (a(this, "deselectButton", { button: this.selectedButtonElement }), this.nextEvent && (this.currentUserDetails && "annotations" === this.currentUserDetails.coll && e.removeAnnotation(this.currentUserDetails), this.mouseMoveEvent = this.nextEvent = !1)); this.selectedButton = c; this.selectedButtonElement = b; a(this, "selectButton", { button: b }); c.init && c.init.call(this, b, d); (c.start || c.steps) && e.renderer.boxWrapper.addClass("highcharts-draw-mode")
                        }; d.prototype.bindingsChartClick = function (b, c) {
                            b = this.chart; var d = this.selectedButton;
                            b = b.renderer.boxWrapper; var e; if (e = this.activeAnnotation && !c.activeAnnotation && c.target.parentNode) { a: { e = c.target; var f = A.Element.prototype, n = f.matches || f.msMatchesSelector || f.webkitMatchesSelector, q = null; if (f.closest) q = f.closest.call(e, ".highcharts-popup"); else { do { if (n.call(e, ".highcharts-popup")) break a; e = e.parentElement || e.parentNode } while (null !== e && 1 === e.nodeType) } e = q } e = !e } e && (a(this, "closePopup"), this.deselectAnnotation()); d && d.start && (this.nextEvent ? (this.nextEvent(c, this.currentUserDetails),
                                this.steps && (this.stepIndex++, d.steps[this.stepIndex] ? this.mouseMoveEvent = this.nextEvent = d.steps[this.stepIndex] : (a(this, "deselectButton", { button: this.selectedButtonElement }), b.removeClass("highcharts-draw-mode"), d.end && d.end.call(this, c, this.currentUserDetails), this.mouseMoveEvent = this.nextEvent = !1, this.selectedButton = null))) : (this.currentUserDetails = d.start.call(this, c), d.steps ? (this.stepIndex = 0, this.steps = !0, this.mouseMoveEvent = this.nextEvent = d.steps[this.stepIndex]) : (a(this, "deselectButton", { button: this.selectedButtonElement }),
                                    b.removeClass("highcharts-draw-mode"), this.steps = !1, this.selectedButton = null, d.end && d.end.call(this, c, this.currentUserDetails))))
                        }; d.prototype.bindingsContainerMouseMove = function (a, b) { this.mouseMoveEvent && this.mouseMoveEvent(b, this.currentUserDetails) }; d.prototype.fieldsToOptions = function (a, b) {
                            n(a, function (a, c) {
                                var d = parseFloat(a), e = c.split("."), f = b, n = e.length - 1; !p(d) || a.match(/px/g) || c.match(/format/g) || (a = d); "" !== a && "undefined" !== a && e.forEach(function (b, c) {
                                    var d = q(e[c + 1], ""); n === c ? f[b] = a : (f[b] ||
                                        (f[b] = d.match(/\d/g) ? [] : {}), f = f[b])
                                })
                            }); return b
                        }; d.prototype.deselectAnnotation = function () { this.activeAnnotation && (this.activeAnnotation.setControlPointsVisibility(!1), this.activeAnnotation = !1) }; d.prototype.annotationToFields = function (a) {
                            function e(d, f, q, g) {
                                if (q && -1 === l.indexOf(f) && (0 <= (q.indexOf && q.indexOf(f)) || q[f] || !0 === q)) if (c(d)) g[f] = [], d.forEach(function (a, b) { m(a) ? (g[f][b] = {}, n(a, function (a, c) { e(a, c, h[f], g[f][b]) })) : e(a, 0, h[f], g[f]) }); else if (m(d)) {
                                    var w = {}; c(g) ? (g.push(w), w[f] = {}, w = w[f]) :
                                        g[f] = w; n(d, function (a, b) { e(a, b, 0 === f ? q : h[f], w) })
                                } else "format" === f ? g[f] = [b(d, a.labels[0].points[0]).toString(), "text"] : c(g) ? g.push([d, k(d)]) : g[f] = [d, k(d)]
                            } var f = a.options, g = d.annotationsEditable, h = g.nestedOptions, k = this.utils.getFieldType, w = q(f.type, f.shapes && f.shapes[0] && f.shapes[0].type, f.labels && f.labels[0] && f.labels[0].itemType, "label"), l = d.annotationsNonEditable[f.langKey] || [], u = { langKey: f.langKey, type: w }; n(f, function (a, b) {
                                "typeOptions" === b ? (u[b] = {}, n(f[b], function (a, c) { e(a, c, h, u[b], !0) })) : e(a,
                                    b, g[w], u)
                            }); return u
                        }; d.prototype.getClickedClassNames = function (a, b) { var c = b.target; b = []; for (var d; c && ((d = k(c, "class")) && (b = b.concat(d.split(" ").map(function (a) { return [a, c] }))), c = c.parentNode, c !== a);); return b }; d.prototype.getButtonEvents = function (a, b) { var c = this, d; this.getClickedClassNames(a, b).forEach(function (a) { c.boundClassNames[a[0]] && !d && (d = { events: c.boundClassNames[a[0]], button: a[1] }) }); return d }; d.prototype.update = function (a) { this.options = t(!0, this.options, a); this.removeEvents(); this.initEvents() };
                        d.prototype.removeEvents = function () { this.eventsToUnbind.forEach(function (a) { a() }) }; d.prototype.destroy = function () { this.removeEvents() }; d.annotationsEditable = {
                            nestedOptions: {
                                labelOptions: ["style", "format", "backgroundColor"], labels: ["style"], label: ["style"], style: ["fontSize", "color"], background: ["fill", "strokeWidth", "stroke"], innerBackground: ["fill", "strokeWidth", "stroke"], outerBackground: ["fill", "strokeWidth", "stroke"], shapeOptions: ["fill", "strokeWidth", "stroke"], shapes: ["fill", "strokeWidth", "stroke"],
                                line: ["strokeWidth", "stroke"], backgroundColors: [!0], connector: ["fill", "strokeWidth", "stroke"], crosshairX: ["strokeWidth", "stroke"], crosshairY: ["strokeWidth", "stroke"]
                            }, circle: ["shapes"], verticalLine: [], label: ["labelOptions"], measure: ["background", "crosshairY", "crosshairX"], fibonacci: [], tunnel: ["background", "line", "height"], pitchfork: ["innerBackground", "outerBackground"], rect: ["shapes"], crookedLine: [], basicAnnotation: []
                        }; d.annotationsNonEditable = { rectangle: ["crosshairX", "crosshairY", "label"] }; return d
                    }();
                    v.prototype.utils = { updateRectSize: function (a, b) { var c = b.chart, d = b.options.typeOptions, e = c.pointer.getCoordinates(a); a = e.xAxis[0].value - d.point.x; d = d.point.y - e.yAxis[0].value; b.update({ typeOptions: { background: { width: c.inverted ? d : a, height: c.inverted ? a : d } } }) }, getFieldType: function (a) { return { string: "text", number: "number", "boolean": "checkbox" }[typeof a] } }; l.Chart.prototype.initNavigationBindings = function () {
                        var a = this.options; a && a.navigation && a.navigation.bindings && (this.navigationBindings = new v(this, a.navigation),
                            this.navigationBindings.initEvents(), this.navigationBindings.initUpdate())
                    }; e(l.Chart, "load", function () { this.initNavigationBindings() }); e(l.Chart, "destroy", function () { this.navigationBindings && this.navigationBindings.destroy() }); e(v, "deselectButton", function () { this.selectedButtonElement = null }); e(d, "remove", function () { this.chart.navigationBindings && this.chart.navigationBindings.deselectAnnotation() }); l.Annotation && (r(d), n(d.types, function (a) { r(a) })); g({
                        lang: {
                            navigation: {
                                popup: {
                                    simpleShapes: "Simple shapes",
                                    lines: "Lines", circle: "Circle", rectangle: "Rectangle", label: "Label", shapeOptions: "Shape options", typeOptions: "Details", fill: "Fill", format: "Text", strokeWidth: "Line width", stroke: "Line color", title: "Title", name: "Name", labelOptions: "Label options", labels: "Labels", backgroundColor: "Background color", backgroundColors: "Background colors", borderColor: "Border color", borderRadius: "Border radius", borderWidth: "Border width", style: "Style", padding: "Padding", fontSize: "Font size", color: "Color", height: "Height", shapes: "Shape options"
                                }
                            }
                        },
                        navigation: {
                            bindingsClassName: "highcharts-bindings-container", bindings: {
                                circleAnnotation: {
                                    className: "highcharts-circle-annotation", start: function (a) { a = this.chart.pointer.getCoordinates(a); var b = this.chart.options.navigation; return this.chart.addAnnotation(t({ langKey: "circle", type: "basicAnnotation", shapes: [{ type: "circle", point: { xAxis: 0, yAxis: 0, x: a.xAxis[0].value, y: a.yAxis[0].value }, r: 5 }] }, b.annotationsOptions, b.bindings.circleAnnotation.annotationsOptions)) }, steps: [function (a, b) {
                                        var c = b.options.shapes[0].point,
                                        d = this.chart.xAxis[0].toPixels(c.x); c = this.chart.yAxis[0].toPixels(c.y); var e = this.chart.inverted; b.update({ shapes: [{ r: Math.max(Math.sqrt(Math.pow(e ? c - a.chartX : d - a.chartX, 2) + Math.pow(e ? d - a.chartY : c - a.chartY, 2)), 5) }] })
                                    }]
                                }, rectangleAnnotation: {
                                    className: "highcharts-rectangle-annotation", start: function (a) {
                                        var b = this.chart.pointer.getCoordinates(a); a = this.chart.options.navigation; var c = b.xAxis[0].value; b = b.yAxis[0].value; return this.chart.addAnnotation(t({
                                            langKey: "rectangle", type: "basicAnnotation", shapes: [{
                                                type: "path",
                                                points: [{ xAxis: 0, yAxis: 0, x: c, y: b }, { xAxis: 0, yAxis: 0, x: c, y: b }, { xAxis: 0, yAxis: 0, x: c, y: b }, { xAxis: 0, yAxis: 0, x: c, y: b }]
                                            }]
                                        }, a.annotationsOptions, a.bindings.rectangleAnnotation.annotationsOptions))
                                    }, steps: [function (a, b) { var c = b.options.shapes[0].points, d = this.chart.pointer.getCoordinates(a); a = d.xAxis[0].value; d = d.yAxis[0].value; c[1].x = a; c[2].x = a; c[2].y = d; c[3].y = d; b.update({ shapes: [{ points: c }] }) }]
                                }, labelAnnotation: {
                                    className: "highcharts-label-annotation", start: function (a) {
                                        a = this.chart.pointer.getCoordinates(a);
                                        var b = this.chart.options.navigation; return this.chart.addAnnotation(t({ langKey: "label", type: "basicAnnotation", labelOptions: { format: "{y:.2f}" }, labels: [{ point: { xAxis: 0, yAxis: 0, x: a.xAxis[0].value, y: a.yAxis[0].value }, overflow: "none", crop: !0 }] }, b.annotationsOptions, b.bindings.labelAnnotation.annotationsOptions))
                                    }
                                }
                            }, events: {}, annotationsOptions: {}
                        }
                    }); return v
                }); t(d, "annotations/popup.js", [d["parts/Globals.js"], d["annotations/navigationBindings.js"], d["parts/Pointer.js"], d["parts/Utilities.js"]], function (d,
                    f, l, g) {
                        var r = g.addEvent, e = g.createElement, k = g.defined, b = g.getOptions, a = g.isArray, c = g.isObject, h = g.isString, p = g.objectEach, m = g.pick; g = g.wrap; var t = /\d/g; g(l.prototype, "onContainerMouseDown", function (a, b) { var c = b.target && b.target.className; h(c) && 0 <= c.indexOf("highcharts-popup-field") || a.apply(this, Array.prototype.slice.call(arguments, 1)) }); d.Popup = function (a, b) { this.init(a, b) }; d.Popup.prototype = {
                            init: function (a, b) {
                                this.container = e("div", { className: "highcharts-popup" }, null, a); this.lang = this.getLangpack();
                                this.iconsURL = b; this.addCloseBtn()
                            }, addCloseBtn: function () { var a = this; var b = e("div", { className: "highcharts-popup-close" }, null, this.container); b.style["background-image"] = "url(" + this.iconsURL + "close.svg)";["click", "touchstart"].forEach(function (c) { r(b, c, function () { a.closePopup() }) }) }, addColsContainer: function (a) {
                                var b = e("div", { className: "highcharts-popup-lhs-col" }, null, a); a = e("div", { className: "highcharts-popup-rhs-col" }, null, a); e("div", { className: "highcharts-popup-rhs-col-wrapper" }, null, a); return {
                                    lhsCol: b,
                                    rhsCol: a
                                }
                            }, addInput: function (a, b, c, d) { var f = a.split("."); f = f[f.length - 1]; var g = this.lang; b = "highcharts-" + b + "-" + f; b.match(t) || e("label", { innerHTML: g[f] || f, htmlFor: b }, null, c); e("input", { name: b, value: d[0], type: d[1], className: "highcharts-popup-field" }, null, c).setAttribute("highcharts-data-name", a) }, addButton: function (a, b, c, d, f) {
                                var g = this, h = this.closePopup, n = this.getFields; var q = e("button", { innerHTML: b }, null, a);["click", "touchstart"].forEach(function (a) { r(q, a, function () { h.call(g); return d(n(f, c)) }) });
                                return q
                            }, getFields: function (a, b) {
                                var c = a.querySelectorAll("input"), d = a.querySelectorAll("#highcharts-select-series > option:checked")[0]; a = a.querySelectorAll("#highcharts-select-volume > option:checked")[0]; var e, f; var g = { actionType: b, linkedTo: d && d.getAttribute("value"), fields: {} };[].forEach.call(c, function (a) { f = a.getAttribute("highcharts-data-name"); (e = a.getAttribute("highcharts-data-series-id")) ? g.seriesId = a.value : f ? g.fields[f] = a.value : g.type = a.value }); a && (g.fields["params.volumeSeriesID"] = a.getAttribute("value"));
                                return g
                            }, showPopup: function () { var a = this.container, b = a.querySelectorAll(".highcharts-popup-close")[0]; a.innerHTML = ""; 0 <= a.className.indexOf("highcharts-annotation-toolbar") && (a.classList.remove("highcharts-annotation-toolbar"), a.removeAttribute("style")); a.appendChild(b); a.style.display = "block" }, closePopup: function () { this.popup.container.style.display = "none" }, showForm: function (a, b, c, d) {
                                this.popup = b.navigationBindings.popup; this.showPopup(); "indicators" === a && this.indicators.addForm.call(this, b, c,
                                    d); "annotation-toolbar" === a && this.annotations.addToolbar.call(this, b, c, d); "annotation-edit" === a && this.annotations.addForm.call(this, b, c, d); "flag" === a && this.annotations.addForm.call(this, b, c, d, !0)
                            }, getLangpack: function () { return b().lang.navigation.popup }, annotations: {
                                addToolbar: function (a, b, c) {
                                    var d = this, f = this.lang, g = this.popup.container, h = this.showForm; -1 === g.className.indexOf("highcharts-annotation-toolbar") && (g.className += " highcharts-annotation-toolbar"); g.style.top = a.plotTop + 10 + "px"; e("span",
                                        { innerHTML: m(f[b.langKey] || b.langKey, b.shapes && b.shapes[0].type) }, null, g); var k = this.addButton(g, f.removeButton || "remove", "remove", c, g); k.className += " highcharts-annotation-remove-button"; k.style["background-image"] = "url(" + this.iconsURL + "destroy.svg)"; k = this.addButton(g, f.editButton || "edit", "edit", function () { h.call(d, "annotation-edit", a, b, c) }, g); k.className += " highcharts-annotation-edit-button"; k.style["background-image"] = "url(" + this.iconsURL + "edit.svg)"
                                }, addForm: function (a, b, c, d) {
                                    var f = this.popup.container,
                                    g = this.lang; e("h2", { innerHTML: g[b.langKey] || b.langKey, className: "highcharts-popup-main-title" }, null, f); var h = e("div", { className: "highcharts-popup-lhs-col highcharts-popup-lhs-full" }, null, f); var k = e("div", { className: "highcharts-popup-bottom-row" }, null, f); this.annotations.addFormFields.call(this, h, a, "", b, [], !0); this.addButton(k, d ? g.addButton || "add" : g.saveButton || "save", d ? "add" : "save", c, f)
                                }, addFormFields: function (b, d, f, g, h, k) {
                                    var q = this, n = this.annotations.addFormFields, l = this.addInput, m = this.lang, r,
                                    x; p(g, function (e, g) { r = "" !== f ? f + "." + g : g; c(e) && (!a(e) || a(e) && c(e[0]) ? (x = m[g] || g, x.match(t) || h.push([!0, x, b]), n.call(q, b, d, r, e, h, !1)) : h.push([q, r, "annotation", b, e])) }); k && (h = h.sort(function (a) { return a[1].match(/format/g) ? -1 : 1 }), h.forEach(function (a) { !0 === a[0] ? e("span", { className: "highcharts-annotation-title", innerHTML: a[1] }, null, a[2]) : l.apply(a[0], a.splice(1)) }))
                                }
                            }, indicators: {
                                addForm: function (a, b, c) {
                                    var d = this.indicators, e = this.lang; this.tabs.init.call(this, a); b = this.popup.container.querySelectorAll(".highcharts-tab-item-content");
                                    this.addColsContainer(b[0]); d.addIndicatorList.call(this, a, b[0], "add"); var f = b[0].querySelectorAll(".highcharts-popup-rhs-col")[0]; this.addButton(f, e.addButton || "add", "add", c, f); this.addColsContainer(b[1]); d.addIndicatorList.call(this, a, b[1], "edit"); f = b[1].querySelectorAll(".highcharts-popup-rhs-col")[0]; this.addButton(f, e.saveButton || "save", "edit", c, f); this.addButton(f, e.removeButton || "remove", "remove", c, f)
                                }, addIndicatorList: function (a, b, c) {
                                    var d = this, f = b.querySelectorAll(".highcharts-popup-lhs-col")[0];
                                    b = b.querySelectorAll(".highcharts-popup-rhs-col")[0]; var g = "edit" === c, h = g ? a.series : a.options.plotOptions, k = this.indicators.addFormFields, q; var n = e("ul", { className: "highcharts-indicator-list" }, null, f); var l = b.querySelectorAll(".highcharts-popup-rhs-col-wrapper")[0]; p(h, function (b, c) {
                                        var f = b.options; if (b.params || f && f.params) {
                                            var m = d.indicators.getNameType(b, c), p = m.type; q = e("li", { className: "highcharts-indicator-list", innerHTML: m.name }, null, n);["click", "touchstart"].forEach(function (c) {
                                                r(q, c, function () {
                                                    k.call(d,
                                                        a, g ? b : h[p], m.type, l); g && b.options && e("input", { type: "hidden", name: "highcharts-id-" + p, value: b.options.id }, null, l).setAttribute("highcharts-data-series-id", b.options.id)
                                                })
                                            })
                                        }
                                    }); 0 < n.childNodes.length && n.childNodes[0].click()
                                }, getNameType: function (a, b) { var c = a.options, e = d.seriesTypes; e = e[b] && e[b].prototype.nameBase || b.toUpperCase(); c && c.type && (b = a.options.type, e = a.name); return { name: e, type: b } }, listAllSeries: function (a, b, c, d, f) {
                                    a = "highcharts-" + b + "-type-" + a; var g; e("label", { innerHTML: this.lang[b] || b, htmlFor: a },
                                        null, d); var h = e("select", { name: a, className: "highcharts-popup-field" }, null, d); h.setAttribute("id", "highcharts-select-" + b); c.series.forEach(function (a) { g = a.options; !g.params && g.id && "highcharts-navigator-series" !== g.id && e("option", { innerHTML: g.name || g.id, value: g.id }, null, h) }); k(f) && (h.value = f)
                                }, addFormFields: function (a, b, c, d) {
                                    var f = b.params || b.options.params, g = this.indicators.getNameType; d.innerHTML = ""; e("h3", { className: "highcharts-indicator-title", innerHTML: g(b, c).name }, null, d); e("input", {
                                        type: "hidden",
                                        name: "highcharts-type-" + c, value: c
                                    }, null, d); this.indicators.listAllSeries.call(this, c, "series", a, d, b.linkedParent && f.volumeSeriesID); f.volumeSeriesID && this.indicators.listAllSeries.call(this, c, "volume", a, d, b.linkedParent && b.linkedParent.options.id); this.indicators.addParamInputs.call(this, a, "params", f, c, d)
                                }, addParamInputs: function (a, b, d, e, f) {
                                    var g = this, h = this.indicators.addParamInputs, k = this.addInput, l; p(d, function (d, n) {
                                        l = b + "." + n; c(d) ? h.call(g, a, l, d, e, f) : "params.volumeSeriesID" !== l && k.call(g, l, e, f,
                                            [d, "text"])
                                    })
                                }, getAmount: function () { var a = 0; this.series.forEach(function (b) { var c = b.options; (b.params || c && c.params) && a++ }); return a }
                            }, tabs: {
                                init: function (a) { var b = this.tabs; a = this.indicators.getAmount.call(a); var c = b.addMenuItem.call(this, "add"); b.addMenuItem.call(this, "edit", a); b.addContentItem.call(this, "add"); b.addContentItem.call(this, "edit"); b.switchTabs.call(this, a); b.selectTab.call(this, c, 0) }, addMenuItem: function (a, b) {
                                    var c = this.popup.container, d = "highcharts-tab-item", f = this.lang; 0 === b && (d +=
                                        " highcharts-tab-disabled"); b = e("span", { innerHTML: f[a + "Button"] || a, className: d }, null, c); b.setAttribute("highcharts-data-tab-type", a); return b
                                }, addContentItem: function () { return e("div", { className: "highcharts-tab-item-content" }, null, this.popup.container) }, switchTabs: function (a) {
                                    var b = this, c; this.popup.container.querySelectorAll(".highcharts-tab-item").forEach(function (d, e) {
                                        c = d.getAttribute("highcharts-data-tab-type"); "edit" === c && 0 === a || ["click", "touchstart"].forEach(function (a) {
                                            r(d, a, function () {
                                                b.tabs.deselectAll.call(b);
                                                b.tabs.selectTab.call(b, this, e)
                                            })
                                        })
                                    })
                                }, selectTab: function (a, b) { var c = this.popup.container.querySelectorAll(".highcharts-tab-item-content"); a.className += " highcharts-tab-item-active"; c[b].className += " highcharts-tab-item-show" }, deselectAll: function () { var a = this.popup.container, b = a.querySelectorAll(".highcharts-tab-item"); a = a.querySelectorAll(".highcharts-tab-item-content"); var c; for (c = 0; c < b.length; c++)b[c].classList.remove("highcharts-tab-item-active"), a[c].classList.remove("highcharts-tab-item-show") }
                            }
                        };
                    r(f, "showPopup", function (a) { this.popup || (this.popup = new d.Popup(this.chart.container, this.chart.options.navigation.iconsURL || this.chart.options.stockTools && this.chart.options.stockTools.gui.iconsURL || "https://code.highcharts.com/8.1.2/gfx/stock-icons/")); this.popup.showForm(a.formType, this.chart, a.options, a.onSubmit) }); r(f, "closePopup", function () { this.popup && this.popup.closePopup() })
                }); t(d, "masters/modules/annotations-advanced.src.js", [], function () { })
});
//# sourceMappingURL=annotations-advanced.js.map