using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

            Bag = new Dictionary<string, double>();
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public Dictionary<string, double> Bag { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Número de telefone")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(Usuario user)
        {
            string userName = await _userManager.GetUserNameAsync(user);
            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Usuario user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            GerarIndicadoresPessoais(user);

            return Page();
        }

        private void GerarIndicadoresPessoais(Usuario user)
        {
            //Bag["n_projs"] = _context.Projeto.ToList().Where(p => AvaliarLider(p, user)).Count();
            Bag["n_prosps"] = _context.Prospeccao.ToList().Where(p => p.Usuario.Id == user.Id).Count();
            Bag["n_propostas"] = _context.Prospeccao.ToList().
                Where(p => p.Usuario.Id == user.Id &&
                p.Status.Any(j => j.Status == StatusProspeccao.ComProposta)).
                Count();
            Bag["n_convertidos"] = _context.Prospeccao.ToList().
                Where(p => p.Usuario.Id == user.Id &&
                p.Status.Any(j => j.Status == StatusProspeccao.Convertida)).
                Count();

            /*Bag["valor_projetos"] = _context.Projeto.
                ToList()
                .Where(p => AvaliarLider(p, user)).
                Select(p => p.ValorAporteRecursos).Sum() * 0.5 + _context.Projeto.
                ToList().Where(p => AvaliarColiderança(p, user)).
                Select(p => p.ValorAporteRecursos).Sum() * 0.3;*/
        }

        /*private static bool AvaliarColiderança(Projeto p, Usuario user)
        {
            for (int i = 1; i < p.Equipe.Count; i++)
            {
                if (p.Equipe[i].Id == user.Id)
                {
                    return true;
                }
            }
            return false;
        }*/

        /*private static bool AvaliarLider(Projeto p, Usuario user)
        {
            try
            {
                return p.Lider.Id == user.Id;
            }
            catch (Exception)
            {
                return false;
            }
        }*/

        public async Task<IActionResult> OnPostAsync()
        {
            Usuario user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Erro inesperado ao alterar o telefone.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Seu perfil foi atualizado";
            return RedirectToPage();
        }
    }
}