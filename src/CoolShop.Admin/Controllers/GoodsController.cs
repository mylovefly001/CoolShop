using System.Threading.Tasks;
using CoolShop.Admin.Attributes;
using CoolShop.Admin.Services;
using CoolShop.Admin.Wrappers;
using CoolShop.Core.Library;
using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Admin.Controllers
{
    public class GoodsController : BaseController
    {
        private GoodsService GoodsService { get; }

        public GoodsController(GoodsService goodsService)
        {
            GoodsService = goodsService;
        }

        /// <summary>
        /// 模型管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Auth(Ctr = "Goods", Act = "List")]
        public IActionResult List()
        {
            return View();
        }

        #region 商品模板

        [HttpGet]
        [Auth(Ctr = "Goods", Act = "TemplateList")]
        public IActionResult TemplateList()
        {
            return View();
        }

        [Auth(Ctr = "Goods", Act = "TemplateList")]
        public async Task<JsonResult> TemplateDo()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "add":
                {
                    var name = GetParam<string>("name").CheckRequired("模板名称不得为空");
                    var sort = GetParam<int>("sort");
                    var status = GetParam<int>("status");
                    await GoodsService.InsertGoodsTemplate(name, sort, status);
                    break;
                }
                case "edit":
                {
                    var id = GetParam<int>("id").CheckId("ID不得为空");
                    var name = GetParam<string>("name").CheckRequired("模板名称不得为空");
                    var sort = GetParam<int>("sort");
                    var status = GetParam<int>("status");
                    await GoodsService.UpdateGoodsTemplate(id, name, sort, status);
                    break;
                }
                case "del":
                {
                    var id = GetParam<int>("id").CheckId("ID不得为空");
                    await GoodsService.DeleteGoodsTemplate(id);
                    break;
                }
                case "list":
                {
                    var rsGoodsTemplateList = await GoodsService.GetGoodsTemplateList();
                    return Result(rsGoodsTemplateList);
                }
            }

            return Result();
        }

        #endregion

        #region 商品属性

        [HttpGet]
        [Auth(Ctr = "Goods", Act = "AttributeList")]
        public async Task<ActionResult> AttributeList()
        {
            ViewBag.rsGoodsTemplateList = await GoodsService.GetGoodsTemplateList();
            return View();
        }

        public async Task<JsonResult> AttributeDo()
        {
            var cmd = GetParam<string>("cmd");
            switch (cmd)
            {
                case "add":
                {
                    var goodsAttributeWrapper = GetParam<GoodsAttributeWrapper>(t => { t.Name.CheckRequired("属性名称不得为空"); });
                    await GoodsService.InsertGoodsAttribute(goodsAttributeWrapper);
                    break;
                }
                case "edit":
                {
                    var id = GetParam<int>("id").CheckId("ID不得为空");
                    var name = GetParam<string>("name").CheckRequired("模板名称不得为空");
                    var sort = GetParam<int>("sort");
                    var status = GetParam<int>("status");
                    await GoodsService.UpdateGoodsTemplate(id, name, sort, status);
                    break;
                }
                case "del":
                {
                    var id = GetParam<int>("id").CheckId("ID不得为空");
                    await GoodsService.DeleteGoodsTemplate(id);
                    break;
                }
                case "list":
                {
                    var rsGoodsTemplateList = await GoodsService.GetGoodsTemplateList();
                    return Result(rsGoodsTemplateList);
                }
            }

            return Result();
        }

        #endregion
    }
}