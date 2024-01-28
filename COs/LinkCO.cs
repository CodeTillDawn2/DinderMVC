
using DinderDLL.DTOs;
using DinderDLL.Services;
using System.Collections.Generic;

namespace DinderDLL.DataModels
{
#pragma warning disable CS1591
    public class LinkCO : DataModel<LinkDTO>
    {
        public string Rel { get; set; }   // Relation to describe the link's purpose
        public string Href { get; set; }  // Target URI of the linked resource
        public string Method { get; set; } // HTTP method (optional, can be used for non-GET requests)
        public bool Templated { get; set; } // Indicates whether the Href contains template variables
        private List<LinkCO> _links = new List<LinkCO>();
        public override List<LinkCO> Links { get => _links; set => _links = value; }


        public LinkCO(string rel, string href)
        {
            Rel = rel;
            Href = href;

            Templated = false;
            if (href.Contains("{?"))
            {
                Templated = true;
            }
            if (rel.ToLower().Contains("get"))
            {
                Method = LinkService.CRUD_Get;
            }
            else if (rel.ToLower().Contains("create"))
            {
                Method = LinkService.CRUD_Post;
            }
            else if (rel.ToLower().Contains("delete"))
            {
                Method = LinkService.CRUD_Delete;
            }
            else if (rel.ToLower().Contains("update"))
            {
                Method = LinkService.CRUD_Put;
            }
            else
            {
                throw new System.Exception($"Unknown link type, may be malformed '{rel}'");
            }

        }


        public override LinkDTO ReturnDTO()
        {
            return new LinkDTO(Rel, Href, Method, Templated);
        }
    }



#pragma warning restore CS1591
}
